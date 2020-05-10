import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { Observable } from 'rxjs/internal/Observable';
import { of } from 'rxjs/internal/observable/of';

import { IBatchOutput, IBatch } from './batch';


@Injectable({
  providedIn: 'root'
})

export class BatchService {


  private getBatchesUrl = 'http://localhost:59933/api/batch/state';
  private processBatchUrl = 'http://localhost:59933/api/batch/processing';
  first: number;
  secong:number;

  allBatches: IBatchOutput;
  constructor(private http: HttpClient) { }



  getBatches(param: any): Observable<IBatchOutput> {

    const httpOptions =
      new HttpHeaders({ 'Content-Type': 'application/json' });

    const params = new HttpParams()
      .set('batchSize', param.batchSize);

    return this.http.get<IBatchOutput>(
      this.getBatchesUrl, { headers: httpOptions, params: params })
      .pipe(
        tap( // Log the result or error
          data =>  console.log(data),
          catchError(this.handleError<IBatch>("get batches"))));
  }


  processBatch(param: any): Observable<any> {    

    const headers: HttpHeaders = new  HttpHeaders();
    headers.set('Content-Type', 'application/x-www-form-urlencoded');
    
    
    var postData = {
      'batchCount': +param.batchSize, 'itemsPerBatch': +param.itemsPerBatch
    };  
 

    return this.http.post<any>(this.processBatchUrl, postData, { headers: headers }).pipe(
      tap(data => console.log(data),
        catchError(this.handleError<IBatch>("process batch"))));


  }


  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  /** Log a HeroService message with the MessageService */
  private log(message: string) {
    console.log(`Batch Service: ${message}`);
  }
}
