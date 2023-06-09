import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { UserCartComponent } from './user-cart/user-cart.component';
import { UserCheckoutComponent } from './user-checkout/user-checkout.component';
import { UserOrdersComponent } from './user-orders/user-orders.component';
import { UserAccountPrivateComponent } from './user-account-private/user-account-private.component';
import { UserUpdateFormComponent } from './user-update-form/user-update-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { UserPasswordFormComponent } from './user-password-form/user-password-form.component';
import { UserPaymentComponent } from './user-payment/user-payment.component';



@NgModule({
  declarations: [
    UserCartComponent,
    UserCheckoutComponent,
    UserOrdersComponent,
    UserAccountPrivateComponent,
    UserUpdateFormComponent,
    UserPasswordFormComponent,
    UserPaymentComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    UserRoutingModule
  ]
})
export class UserModule { }
