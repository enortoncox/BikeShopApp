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
  selector: 'app-review-create-form',
  templateUrl: './review-create-form.component.html',
  styleUrls: ['./review-create-form.component.css']
})
export class ReviewCreateFormComponent implements OnInit, IDeactivateComponent{

  reviewForm!: FormGroup;
  review: Review = {} as Review;
  productId: string = "";
  product: Product = {} as Product;
  currentUser: User = {} as User;
  isSubmitting: boolean;

  constructor(private reviewsService: ReviewsService, private route: ActivatedRoute, private router: Router, private productsService: ProductsService, private authService: AuthService) 
  {
    this.productId = this.route.snapshot.params['productId'];
    this.isSubmitting = false;
  }
  
  ngOnInit(): void {
    this.reviewForm = new FormGroup(({
      title: new FormControl("", Validators.required),
      text: new FormControl("", Validators.required),
      rating: new FormControl("", Validators.required),
    }))

    this.productsService.GetProduct(this.productId).subscribe(productData => this.product = productData);

    this.authService.GetCurrentUser().subscribe(userData => this.currentUser = userData);
  }

  onSubmit()
  {
    this.review.title = this.reviewForm.value.title;
    this.review.text = this.reviewForm.value.text;
    this.review.rating = this.reviewForm.value.rating;
    this.review.productId = this.productId;
    this.review.userId = this.currentUser.userId;

    this.reviewsService.CreateReview(this.review).subscribe(() => 
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
