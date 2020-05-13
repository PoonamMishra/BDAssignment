import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviousBatchComponent } from './previous-batch.component';

describe('PreviousBatchComponent', () => {
  beforeEach(async(() => { // 3
    TestBed.configureTestingModule({ // 4
      declarations: [
        PreviousBatchComponent
      ],
    }).compileComponents(); // 5
  }));
  //
});
