import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { EmployeeCreateComponent } from './employee-create/employee-create.component';
import { ListViewComponent } from './list-view/list-view.component';

 

export const routes: Routes = [
  { path: '', component: ListViewComponent },

  { path: 'employee-list', component: ListViewComponent },
  
  { path: 'employeecreate', component: EmployeeCreateComponent }, // Remove the leading '/',
  { path: '', redirectTo: 'employee-list', pathMatch: 'full' },
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
