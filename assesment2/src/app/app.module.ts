import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule, routes } from './app-routing.module';
import { AppComponent } from './app.component';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';  
import { HttpClientModule } from '@angular/common/http';
import { EmployeeCreateComponent } from './employee-create/employee-create.component';
import { ListViewComponent } from './list-view/list-view.component';
import { ToastrModule } from 'ngx-toastr';
@NgModule({
  declarations: [
    AppComponent,
    EmployeeCreateComponent,
    ListViewComponent  
  ],
  imports: [
    RouterModule.forRoot(routes),
    BrowserAnimationsModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserModule,
    
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    })
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
