import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlComponent } from './control/control.component';
import { AdminRoutingModule } from './admin-routing.module';
import { UserUpdateAdminFormComponent } from './user-update-admin-form/user-update-admin-form.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    ControlComponent,
    UserUpdateAdminFormComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
