import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from 'src/app/interfaces/product';
import { Review } from 'src/app/interfaces/review';
import { User } from 'src/app/interfaces/user';
import { IDeactivateComponent } from 'src/app/routeGuards/form-leave.guard';
import { AuthService } from 'src/app/services/auth.service';
import { ProductsService } from 'src/app/services/products.service';
import { ReviewsService } from 'src/app/services/reviews.service';

@Component({
  selector: 'app-review-update-form',
  templateUrl: './review-update-form.component.html',
  styleUrls: ['./review-update-form.component.css']
})
export class ReviewUpdateFormComponent implements OnInit, IDeactivateComponent{
  reviewForm!: FormGroup;
  product: Product = {} as Product;
  productId: string;
  review!: Review;
  currentUser: User = {} as User;
  isSubmitting: boolean;

  constructor(private reviewsService: ReviewsService, private productsService: ProductsService, private router: Router, private route: ActivatedRoute, private authService: AuthService)
  {
    this.productId = this.route.snapshot.params['productId'];
    this.review = this.route.snapshot.data['reviewData'];
    this.isSubmitting = false;
  }

  ngOnInit(): void {
    this.reviewForm = new FormGroup({
      title: new FormControl(this.review.title, Validators.required),
      text: new FormControl(this.review.text, Validators.required),
      rating: new FormControl(this.review.rating, Validators.required)
    });

    this.productsService.GetProduct(this.productId).subscribe(productData => this.product = productData);

    this.authService.GetCurrentUser().subscribe(userData => this.currentUser = userData);
  }

  onSubmit()
  {
    this.review.title = this.reviewForm.value.title;
    this.review.text = this.reviewForm.value.text;
    this.review.rating = this.reviewForm.value.rating;
    this.review.userId = this.currentUser.userId;
    this.review.productId = this.productId;

    this.reviewsService.UpdateReview(this.review).subscribe(() => 
    {
      this.productsService.SetAvgRatingOfProduct(this.productId).subscribe(() => {
        this.isSubmitting = true;
        this.router.navigate(['/products', this.productId]);
      })
    })
  }

  canExit()
  {
    if((!this.reviewForm.pristine) && this.isSubmitting == false)
    {
      return confirm("You have unsaved changes. Are you sure you want to leave?");
    }
    return true;
  }
}
