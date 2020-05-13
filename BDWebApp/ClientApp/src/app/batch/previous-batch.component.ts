import { Component } from '@angular/core';
import { IBatch } from './batch';
import { BatchService } from './batch.service';



@Component({
  selector: 'app-previous-batch-component',
  templateUrl: './previous-batch.component.html'


})

export class PreviousBatchComponent  {

  batchList: IBatch[];


  constructor(
      private batchService: BatchService) {

  
  }
  ngOnInit() {
    this.previousBatchClick();
  }



  previousBatchClick() {

    this.batchService.getPreviousBatches()
      .subscribe(
        prevBatch => {
          this.batchList = prevBatch.batchList;         

        },
        error => {
          console.log("Error", error);
        }
      );

  }

}
