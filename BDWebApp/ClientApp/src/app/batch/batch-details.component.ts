import { Component, Input } from '@angular/core';
import { IBatch } from './batch';



@Component({
  selector: 'app-batch-details-component',
  templateUrl: './batch-details.component.html'

})

export class BatchDetailsComponent {

  @Input() batchList: IBatch[];
  total: number;


  getTotal() {

    this.total = 0;
    if (this.batchList.length !== 0) {
      this.total = this.batchList.reduce((subtotal, item) => subtotal + item.total, 0)
    }
    return this.total;
  }

}
