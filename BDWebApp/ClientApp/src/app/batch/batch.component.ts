import { OnInit, Component, OnDestroy } from '@angular/core';
import { interval } from 'rxjs/internal/observable/interval';
import { startWith, switchMap, takeWhile } from "rxjs/operators";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RangeValidator } from '../custom-validators/range.validator';
import { IBatchOutput, IBatch } from './batch';
import { BatchService } from './batch.service';
import { timer } from 'rxjs/internal/observable/timer';



@Component({
  selector: 'app-batch-component',
  templateUrl: './batch.component.html',
  styleUrls: ['./batch.component.css'],

})


export class BatchComponent implements OnInit, OnDestroy {

  batchInputForm: FormGroup;
  submitted = false;
  pollingData: any;
  value: any = "";
  pollingFreq: number;
  pollingCount: number;
  batchList: IBatch[];
  startButtonClicked = false;
  isProcessCompleted = false;

  currentGroupId: number = 1;
  PreviousGroupId: number = 0;


  constructor(
    private formBuilder: FormBuilder,
    private batchService: BatchService) {


  }

  ngOnInit() {
    this.batchInputForm = this.formBuilder.group({
      batchSize: ['', [Validators.required, RangeValidator.validateRange]],
      itemsPerBatch: ['', [Validators.required, RangeValidator.validateRange]]
    });
  }


  disableStart(): boolean {
    return (!this.batchInputForm.valid && !this.isProcessCompleted);
  }

  pollValues(): any {

    //let params  = { groupId: this.currentGroupId, batchSize: this.batchInputForm.get('batchSize').value };

    let params = { batchSize: this.batchInputForm.get('batchSize').value };

    this.pollingData = timer(0, 3000)
      .pipe(
        switchMap(batchOutputResult => this.batchService.getBatches(params))
      )
      .subscribe(
        res => {
          this.batchList = res.batchList;
          if (res.isProcessCompleted) {
            this.submitted = false;
            this.isProcessCompleted = true;
            this.pollingData.unsubscribe();

          }

        },
        error => {
          console.log("Error", error);
        }
      );
  }

  toggleButtonText() {

    return (this.submitted === false && this.isProcessCompleted === false) || this.isProcessCompleted === true ? 'Start' : 'Processing';
  }

  

  previousBatchClick() {

    this.batchService.getPreviousBatches()     
      .subscribe(
        res => {
          this.batchList = res.batchList;
          if (res.isProcessCompleted) {
            this.submitted = false;
            this.isProcessCompleted = true;
            this.pollingData.unsubscribe();

          }

        },
        error => {
          console.log("Error", error);
        }
      );

  }



  ngOnDestroy() {
    this.batchList = [];
    this.pollingData.unsubscribe();
  }

  // convenience getter for easy access to form fields
  get f() { return this.batchInputForm.controls; }


  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.batchInputForm.invalid) {
      return;
    }
    this.batchInputForm.get('batchSize')
    //alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.batchInputForm.value))
    this.processBatch();
    this.pollValues();

    
  }
 

  processBatch() {
       

    let params = {
      batchSize: this.batchInputForm.get('batchSize').value,
      itemsPerBatch: this.batchInputForm.get('itemsPerBatch').value
    };
   
    this.batchService.processBatch(params)
      .subscribe(
        data => {
          console.log("Process Request is successful ", data);
        },
        error => {
          console.log("Error", error);

        }

      );

  }

  intiatePolling() {
    this.batchList = [];
    this.startButtonClicked = true;
    this.pollValues();

  }

}
