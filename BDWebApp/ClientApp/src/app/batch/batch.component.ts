import { OnInit, Component, OnDestroy } from '@angular/core';
import { interval } from 'rxjs/internal/observable/interval';
import { startWith, switchMap, takeWhile } from "rxjs/operators";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RangeValidator } from '../custom-validators/range.validator';
import { IBatch } from './batch';
import { BatchService } from './batch.service';



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


  constructor(
    private formBuilder: FormBuilder,
    private batchService: BatchService) {

    //this.PollValues();
    this.getSmartphones();

  }


  getSmartphones() {
    this.batchService.getBatches()
      .subscribe(resp => {
        console.log(resp);
        

        for (const data of resp.body) {
          console.log(data);
        }
        
      });
  }


  PollValues(): any {
    let count = 0;
    this.pollingData = interval(5000)
      .pipe(
        startWith(0),
        //takeWhile(() => this.alive),
        switchMap(batch => this.batchService.getBatches() + "s")
      )
      .subscribe(
        res => {

          var resu = res;
          //count += 1;
          //this.value += count + ",";
          //if (count > 16) {
          //  this.pollingData.unsubscribe();
          //}
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

  }


}
