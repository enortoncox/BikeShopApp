import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Product } from 'src/app/interfaces/product';
import { Review } from 'src/app/interfaces/review';
import { ReviewProduct } from 'src/app/interfaces/reviewProduct';
import { User } from 'src/app/interfaces/user';
import { ProductsService } from 'src/app/services/products.service';
import { ReviewsService } from 'src/app/services/reviews.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-user-account-public',
  templateUrl: './user-account-public.component.html',
  styleUrls: ['./user-account-public.component.css']
})
export class UserAccountPublicComponent implements OnInit{

  user: User;
  reviews: Review[] = [];
  reviewsProducts: ReviewProduct[] = [];
  domain: string = "";
  noImage: string = "";

  constructor(private route: ActivatedRoute, private reviewService: ReviewsService, private productsService: ProductsService)
  {
      this.user = this.route.snapshot.data['userData'];
      this.domain = environment.domain;
      this.noImage = environment.noImage;
  }

  ngOnInit(): void {
    this.reviewService.GetAllReviewsFromAUser(this.user.userId).subscribe(reviewsData => 
      {
        this.reviews = reviewsData;
        this.reviews.forEach(review => 
          {
            let product: Product;
            this.productsService.GetProduct(review.productId).subscribe(productData => 
              {
                product = productData;
                this.reviewsProducts.push({review: review, product: product});
              })
          })
      });
  }

  noImageFound(e: any){
    e.target.src = this.noImage;
  }
}
