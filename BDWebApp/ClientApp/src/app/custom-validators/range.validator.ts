import { Injectable } from '@angular/core';
import { AbstractControl, FormControl } from '@angular/forms';


export class RangeValidator {

  constructor() { }

  static validateRange(control: FormControl) {
    if (control.value != null && (control.value > 10 || control.value < 1)) {
      return { 'invalidRange': true };
    }
    return false;

  }

}
