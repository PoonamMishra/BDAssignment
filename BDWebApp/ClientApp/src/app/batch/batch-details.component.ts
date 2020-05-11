import { Component, Input } from '@angular/core';
import { IBatch } from './batch';



@Component({
  selector: 'app-batch-details-component',
  templateUrl: './batch-details.component.html'

})

export class BatchDetailsComponent {

  @Input() batchList: IBatch[];;
  


}
