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
  startButtonClicked= false;


  constructor(
    private formBuilder: FormBuilder,
    private batchService: BatchService) {
   

  }


  pollValues(): any {


    this.pollingData = timer(0, 3000)
      .pipe(
        switchMap(batchOutputResult => this.batchService.getBatches())
      )
      .subscribe(
        res => {

          var resu = res;
          this.batchList = res.BatchList;
          if (res.isProcessCompleted) {
            this.pollingData.unsubscribe();
          }

        },
        error => {

        }
      );
  }

  ngOnInit() {
    this.batchInputForm = this.formBuilder.group({
      batchSize: ['', [Validators.required, RangeValidator.validateRange]],
      itemsPerBatch: ['', [Validators.required, RangeValidator.validateRange]]
    });
  }

  ngOnDestroy() {
    this.batchList= [];
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
    
    alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.batchInputForm.value))
     this. intiatePolling();
  }

  

  intiatePolling(){
    this.batchList= [];
    this.startButtonClicked = true;
    this.pollValues();

  }

}
