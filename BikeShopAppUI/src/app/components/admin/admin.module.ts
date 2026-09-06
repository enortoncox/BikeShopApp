import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlComponent } from './control/control.component';
import { AdminRoutingModule } from './admin-routing.module';
import { UserUpdateAdminFormComponent } from './user-update-admin-form/user-update-admin-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { IsAdminGuard } from 'src/app/routeGuards/is-admin.guard';
import { IsLoggedInGuard } from 'src/app/routeGuards/is-logged-in.guard';
import { JwtLoginGuard } from 'src/app/routeGuards/jwt-login.guard';

@NgModule({
  declarations: [
    ControlComponent,
    UserUpdateAdminFormComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AdminRoutingModule
  ],
  providers: [IsAdminGuard, IsLoggedInGuard, JwtLoginGuard]
})
export class AdminModule { }
