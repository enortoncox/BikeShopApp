import { Component} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Cart } from 'src/app/interfaces/cart';
import { OrderNew } from 'src/app/interfaces/orderNew';
import { Product } from 'src/app/interfaces/product';
import { User } from 'src/app/interfaces/user';
import { CartService } from 'src/app/services/cart.service';
import { OrdersServices } from 'src/app/services/orders.service';
import { ProductsService } from 'src/app/services/products.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-user-checkout',
  templateUrl: './user-checkout.component.html',
  styleUrls: ['./user-checkout.component.css']
})
export class UserCheckoutComponent{

  userCart: Cart;
  cartProducts$: Observable<Product[]>
  currentUser: User;
  order: OrderNew = {} as OrderNew;
  productsIds: string[] = [];
  domain: string = "";
  noImage: string = "";

  constructor(private cartService: CartService, private route: ActivatedRoute, private router: Router, private ordersService: OrdersServices, private productsService: ProductsService)
  {
    this.domain = environment.domain;
    this.noImage = environment.noImage;
    this.userCart = this.route.snapshot.data['userCart'];
    this.currentUser = this.route.snapshot.data['currentUser'];
    this.cartProducts$ = this.cartService.GetProductsInCart(this.userCart.cartId);
  }

  removeProductFromCart(productId: string)
  {
    this.cartService.RemoveProductFromCart(this.userCart.cartId, productId).subscribe(() => {
      this.cartProducts$ = this.cartService.GetProductsInCart(this.userCart.cartId);
      this.cartService.GetCart(this.userCart.cartId).subscribe(cartData => this.userCart = cartData);
    });
  }

  purchaseItems()
  {
    this.order = {
      numOfItems: this.userCart.totalQuantity,
      orderedDate: new Date().toISOString(),
      totalPrice: this.userCart.totalCost,
      userId: this.currentUser.userId,
    }

    this.cartService.GetProductsInCart(this.userCart.cartId).subscribe((productsData) => 
    {
      productsData.forEach(product => {
        this.productsIds.push(product.productId);
        this.productsService.DecreaseQuantity(product.productId).subscribe(() => {});
      });

      this.ordersService.CreateOrder(this.order, this.productsIds).subscribe(() => {
        this.cartService.EmptyCart(this.userCart.cartId).subscribe(() => {});
        this.router.navigate(['user/checkout/payment']);
      })
    });
  }

  noImageFound(e: any){
    e.target.src = this.noImage;
  }
}


