import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReviewCreateFormComponent } from './review-create-form/review-create-form.component';
import { ReviewsRoutingModule } from './reviews-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { ReviewUpdateFormComponent } from './review-update-form/review-update-form.component';
import { UserAccountPublicComponent } from './user-account-public/user-account-public.component';

@NgModule({
  declarations: [ReviewCreateFormComponent, ReviewUpdateFormComponent, UserAccountPublicComponent],
  imports: [
    CommonModule,
    ReviewsRoutingModule,
    ReactiveFormsModule
  ]
})
export class ReviewsModule { }
