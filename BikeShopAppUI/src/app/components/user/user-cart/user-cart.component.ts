import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Cart } from 'src/app/interfaces/cart';
import { Product } from 'src/app/interfaces/product';
import { CartService } from 'src/app/services/cart.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-user-cart',
  templateUrl: './user-cart.component.html',
  styleUrls: ['./user-cart.component.css']
})
export class UserCartComponent{
  cartProducts$!: Observable<Product[]>;
  userCart: Cart;
  returnTo: string;
  domain: string = "";
  noImage: string = "";

  constructor(private cartService: CartService, private route: ActivatedRoute)
  {
    this.domain = environment.domain;
    this.noImage = environment.noImage;

    this.userCart = this.route.snapshot.data['userCart'];
    this.returnTo = this.route.snapshot.queryParams['returnTo'];

    if(this.returnTo == undefined)
    {
      this.returnTo = "/home";
    }

    this.cartProducts$ = this.cartService.GetProductsInCart(this.userCart.cartId);
  }

  removeProductFromCart(productId: string)
  {
    this.cartService.RemoveProductFromCart(this.userCart.cartId, productId).subscribe(() => {
      this.cartProducts$ = this.cartService.GetProductsInCart(this.userCart.cartId);
      this.cartService.GetCart(this.userCart.cartId).subscribe(cartData => this.userCart = cartData);
    });
  }

  emptyCart()
  {
      this.cartService.EmptyCart(this.userCart.cartId).subscribe(() => 
      {
        this.cartProducts$ = this.cartService.GetProductsInCart(this.userCart.cartId);
        this.cartService.GetCart(this.userCart.cartId).subscribe(cartData => this.userCart = cartData);
      });
  }

  noImageFound(e: any){
    e.target.src = this.noImage;
  }
}
