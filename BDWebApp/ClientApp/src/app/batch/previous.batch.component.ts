import { OnInit, Component, OnDestroy } from '@angular/core';
import { interval } from 'rxjs/internal/observable/interval';
import { startWith, switchMap, takeWhile } from "rxjs/operators";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RangeValidator } from '../custom-validators/range.validator';
import { IBatchOutput, IBatch } from './batch';
import { BatchService } from './batch.service';
import { timer } from 'rxjs/internal/observable/timer';



@Component({
  selector: 'app-previous-batch-component',
  templateUrl: './previous-batch.component.html'


})

export class PreviousBatchComponent  {

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
      private batchService: BatchService) {


  }



  //disableStart(): boolean {
  //  return (!this.batchInputForm.valid && !this.isProcessCompleted);
  //}


  //toggleButtonText() {

  //  return (this.submitted === false && this.isProcessCompleted === false) || this.isProcessCompleted === true ? 'Start' : 'Processing';
  //}



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

}
