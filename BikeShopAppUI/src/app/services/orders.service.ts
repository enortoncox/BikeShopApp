import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Order } from "../interfaces/order";
import { OrderNew } from "../interfaces/orderNew";
import { OrdersResponse } from "../interfaces/ordersResponse";
import { Product } from "../interfaces/product";
import { environment } from "src/environments/environment.development";

@Injectable({
    providedIn: "root"
})
export class OrdersServices
{
    apiURL = environment.domain + "api/orders";

    constructor(private http: HttpClient) {}

    GetOrdersFromUser(userId: string): Observable<Order[]>
    {
        return this.http.get<Order[]>(`${this.apiURL}/users/${userId}`);
    }

    GetOrdersFromUserByPage(userId: string, currentPage: number, pageResults: number): Observable<OrdersResponse>
    {
        return this.http.get<OrdersResponse>(`${this.apiURL}/users/${userId}/pages`, {params: {"currentPage": currentPage, "pageResults": pageResults}});
    }

    CreateOrder(order: OrderNew, productsIds: string[]): Observable<any>
    {
        console.log(productsIds);
        return this.http.post(`${this.apiURL}`, {order, productsIds});
    }

    GetAllProductsInOrder(orderId: string): Observable<Product[]>
    {
        return this.http.get<Product[]>(`${this.apiURL}/${orderId}/products`);
    }
}