import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';

import { IBatchOutput } from './batch';
import {  HttpResponse } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class BatchService {

  //private batchUrl = 'api/batches.json';
  private batchUrl = 'http://localhost:59933/api/batchstate';
  allBatches: IBatchOutput;
  constructor(private http: HttpClient) { }


  getBatches(): Observable<IBatchOutput> {
    let headers = new HttpHeaders({
          'Content-Type': 'application/json',
      });
      let httpOptions = {
          headers: headers,
          withCredentials: true,
      };
    return  this.http.get<IBatchOutput>(
      this.batchUrl)
      .pipe(
        tap( // Log the result or error
          data =>console.log(data),
          error => console.log(error)
        )
      );
    



      
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
