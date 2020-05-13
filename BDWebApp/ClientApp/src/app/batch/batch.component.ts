import { OnInit, Component, OnDestroy } from '@angular/core';
import { switchMap } from "rxjs/operators";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RangeValidator } from '../custom-validators/range.validator';
import { IBatch } from './batch';
import { BatchService } from './batch.service';
import { timer } from 'rxjs/internal/observable/timer';
import { Router } from '@angular/router';



@Component({
  selector: 'app-batch-component',
  templateUrl: './batch.component.html'
})


export class BatchComponent implements OnInit, OnDestroy {

  batchInputForm: FormGroup;
  submitted = false;
  pollingData: any;
  batchList: IBatch[];
  startButtonClicked = false;
  isProcessCompleted = true;

  currentGroupId: number = 1;
  PreviousGroupId: number = 0;
  showPreviousBatch = false;


  constructor(
    private formBuilder: FormBuilder,
    private batchService: BatchService,
    private router: Router) {


  }

  ngOnInit() {
    this.batchInputForm = this.formBuilder.group({
      batchSize: ['', [Validators.required, RangeValidator.validateRange]],
      itemsPerBatch: ['', [Validators.required, RangeValidator.validateRange]]
    });

    this.getCurrentGroupId();


  }


  setShowPreviousBatch(groupId) {
    if (groupId >= 2) {
      this.showPreviousBatch = true;
    }
  }

  disableStart(): boolean {
    return (!this.batchInputForm.valid || !this.isProcessCompleted);
  }

  enablePreviousBatch(): boolean {
    return !this.isProcessCompleted;
  }

  toggleButtonText() {
    return this.isProcessCompleted === true ? 'Start' : 'Processing';

  }

  onPreviousAction() {
    this.router.navigate(['/previous-batch'])
      .then(success => console.log('navigation success?', success))
      .catch(console.error);
  }

  getCurrentGroupId() {
    this.batchService.getCurrentGroupId()
      .subscribe(
        groupId => {
          this.currentGroupId = groupId;
          this.setShowPreviousBatch(this.setShowPreviousBatch(this.currentGroupId))
        },
        error => {
          console.log("Error", error);
        }
      );

  }

  
  // convenience getter for easy access to form fields
  get f() { return this.batchInputForm.controls; }


  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.batchInputForm.invalid) {
      return;
    }
    this.isProcessCompleted = false;
    this.batchList = [];
    this.batchInputForm.get('batchSize')
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

  pollValues(): any {

    //let params  = { groupId: this.currentGroupId, batchSize: this.batchInputForm.get('batchSize').value };

    let params = { batchSize: this.batchInputForm.get('batchSize').value };

    this.pollingData = timer(0, 3000)
      .pipe(
        switchMap(() => this.batchService.getBatches(params))
      )
      .subscribe(
        res => {
          this.batchList = res['BatchList'];
          if (res['IsProcessCompleted']) {
            this.submitted = false;
            this.isProcessCompleted = true;
            this.pollingData.unsubscribe();

          }
          this.setShowPreviousBatch(res['CurrentGroupId']);

        },
        error => {
          console.log("Error", error);
        }
      );
  }

  ngOnDestroy() {
    this.batchList = [];
    if (this.pollingData != null) {
      this.pollingData.unsubscribe();
    }
  }
}
