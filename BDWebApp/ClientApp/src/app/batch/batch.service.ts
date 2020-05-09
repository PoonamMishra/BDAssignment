import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';

import { IBatch } from './batch';
import {  HttpResponse } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class BatchService {

  //private batchUrl = 'api/batches.json';
  private batchUrl = 'http://localhost:59933/api/batchstate';
  allBatches: IBatch[];
  constructor(private http: HttpClient) { }


  getBatches(): Observable<HttpResponse<IBatch[]>> {
    return  this.http.get<IBatch[]>(
      this.batchUrl, { observe: 'response' });
    
  }

  //getBatches(): Observable<IBatch[]> {




  //  var result = this.http.get<IBatch[]>(this.batchUrl)
  //    .pipe(
  //      tap(data => {
  //        console.log('All: ' + JSON.stringify(data));
  //        this.allBatches = data
  //      },

  //        catchError(this.handleError))
  //  );
  //  return result;
  //}

  //getBatches(): Observable<IBatch[]> {

  //  return this.http.get<IBatch[]>
  //    (this.batchUrl).pipe(tap(res => this.allBatches = res))
  //} 



  //getProduct(id: number): Observable<IProduct | undefined> {
  //  return this.getProducts()
  //    .pipe(
  //      map((products: IProduct[]) => products.find(p => p.productId === id))
  //    );
  //}

  private handleError(err: HttpErrorResponse) {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }
}
