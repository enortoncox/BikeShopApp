import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from 'src/app/interfaces/product';
import { Review } from 'src/app/interfaces/review';
import { ReviewUser } from 'src/app/interfaces/reviewUser';
import { User } from 'src/app/interfaces/user';
import { AuthService } from 'src/app/services/auth.service';
import { CartService } from 'src/app/services/cart.service';
import { ReviewsService } from 'src/app/services/reviews.service';
import { UsersService } from 'src/app/services/users.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit{

  product: Product;
  reviews: Review[] = [];
  currentUser: User = {} as User;
  reviewsUsers: ReviewUser[] = [];
  isLoggedIn: boolean = false;
  domain: string = "";
  noImage: string = "";

  constructor(private usersService: UsersService, private route: ActivatedRoute, private router: Router, private reviewsService: ReviewsService, private authService: AuthService, private cartService: CartService)
  {
    this.product = this.route.snapshot.data['productData'];
    this.isLoggedIn = authService.isLoggedIn;
    this.domain = environment.domain;
    this.noImage = environment.noImage;
  }

  ngOnInit(): void {
    this.reviewsService.GetReviewsOfProduct(this.product.productId).subscribe(reviewsData => {
      this.reviews = reviewsData;
      this.reviews.forEach(review => 
        {
          this.usersService.GetUser(review.userId).subscribe(userData => 
            {
              this.reviewsUsers.push({review: review, user: userData});
            })
        })
    });

    this.authService.GetCurrentUser().subscribe(userData => this.currentUser = userData);
  }

  deleteReview(reviewId: string)
  {
    this.reviewsService.DeleteReview(reviewId).subscribe(() => {
      this.reviewsUsers = this.reviewsUsers.filter(reviewUser => reviewUser.review.reviewId != reviewId);
    });
  }

  addProductToCart(){
      if(this.isLoggedIn){
        this.cartService.AddToCart(this.currentUser.cartId, this.product.productId).subscribe(() => 
        {
          this.router.navigate(['/user/cart'], {queryParams: {returnTo: `/products/${this.product.productId}`}});
        });
      }
      else
      {
        this.router.navigate(['/auth/login']);
      }
  }

  buyNow()
  {
    if(this.isLoggedIn){
      this.cartService.AddToCart(this.currentUser.cartId, this.product.productId).subscribe(() => 
      {
        this.router.navigate(['/user/checkout']);
      });
    }
    else
    {
      this.router.navigate(['/auth/login']);
    }
  }

  noImageFound(e: any){
    e.target.src = this.noImage;
  }
}
