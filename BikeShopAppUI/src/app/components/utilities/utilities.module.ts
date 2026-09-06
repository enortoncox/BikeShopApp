import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductItemComponent } from './product-item/product-item.component';
import { RouterModule } from '@angular/router';
import { ErrorModalComponent } from './error-modal/error-modal.component';

@NgModule({
  declarations: [
    ProductItemComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [ProductItemComponent]
})
export class UtilitiesModule { }
