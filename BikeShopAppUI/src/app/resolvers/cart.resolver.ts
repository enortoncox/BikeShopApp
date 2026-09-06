import { Injectable } from '@angular/core';
import { RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Cart } from '../interfaces/cart';
import { User } from '../interfaces/user';
import { AuthService } from '../services/auth.service';
import { CartService } from '../services/cart.service';

@Injectable({
  providedIn: 'root'
})
export class CartResolver  {

  currentUser: User = {} as User;

  constructor(private cartService: CartService, private authService: AuthService){}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Cart> {

    this.authService.GetCurrentUser().subscribe(userData => this.currentUser = userData);

    return this.cartService.GetCart(this.currentUser.cartId);
  }
}
