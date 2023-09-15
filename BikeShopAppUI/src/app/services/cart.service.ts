import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Cart } from "../interfaces/cart";
import { Product } from "../interfaces/product";
import { environment } from "src/environments/environment.development";

@Injectable({
    providedIn: "root"
})
export class CartService
{
    apiUrl = environment.domain + "api/carts"

    constructor(private http: HttpClient) {}

    GetCart(cartId: string): Observable<Cart>
    {
        return this.http.get<Cart>(`${this.apiUrl}/${cartId}`);
    }

    GetProductsInCart(cartId: string): Observable<Product[]>
    {
        return this.http.get<Product[]>(`${this.apiUrl}/${cartId}/products`);
    }

    AddToCart(cartId: string, productId: string): Observable<any>
    {
        return this.http.post<any>(`${this.apiUrl}/${cartId}/products`, {cartId, productId});
    }

    EmptyCart(cartId: string): Observable<any>
    {
        return this.http.delete(`${this.apiUrl}/${cartId}`);
    }

    RemoveProductFromCart(cartId: string, productId: string): Observable<any>
    {
        return this.http.delete(`${this.apiUrl}/${cartId}/products/${productId}`);
    }
}