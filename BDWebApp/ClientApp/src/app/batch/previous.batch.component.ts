import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { IBatch } from './batch';
import { BatchService } from './batch.service';



@Component({
  selector: 'app-previous-batch-component',
  templateUrl: './previous.batch.component.html'


})

export class PreviousBatchComponent  {

  batchList: IBatch[];


  constructor(
      private batchService: BatchService) {

  
  }
  ngOnInit() {
    this.previousBatchClick();
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

        },
        error => {
          console.log("Error", error);
        }
      );

  }

}
