import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { BatchComponent } from './batch/batch.component';
import { PreviousBatchComponent } from './batch/previous-batch.component';
import { BatchDetailsComponent } from './batch/batch-details.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    BatchComponent,
    PreviousBatchComponent,
    BatchDetailsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: BatchComponent, pathMatch: 'full' },
      { path: 'batch', component: BatchComponent },
      { path: 'previous-batch', component: PreviousBatchComponent },
     
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
