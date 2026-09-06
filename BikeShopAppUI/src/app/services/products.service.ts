import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../interfaces/product';
import { ProductsResponse } from '../interfaces/productsResponse';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  apiUrl: string = environment.domain + "api/products"

  constructor(private http: HttpClient) {}

  GetProduct(productId: string): Observable<Product>
  {
    return this.http.get<Product>(`${this.apiUrl}/${productId}`);
  }

  GetProducts(categoryId: string): Observable<Product[]>
  {
    return this.http.get<Product[]>(this.apiUrl, {params: {"categoryId": categoryId}});
  }

  GetProductsByPage(categoryId: string = "1", currentPage: number, pageResults: number): Observable<ProductsResponse>
  {
    return this.http.get<ProductsResponse>(`${this.apiUrl}/pages`, {params: {"categoryId": categoryId, "currentPage": currentPage, "pageResults": pageResults}});
  }

  GetFilteredProductsByPage(categoryId: string  = "1", currentPage: number, pageResults: number, price: string, rating: string): Observable<ProductsResponse>
  {
    return this.http.get<ProductsResponse>(`${this.apiUrl}/pages/filter`, {params: {"categoryId": categoryId, "currentPage": currentPage, "pageResults": pageResults, "price": price, "rating": rating}});
  }

  CreateProduct(product: Product): Observable<any>
  {
    return this.http.post(`${this.apiUrl}`, product);
  }

  UpdateProduct(product: Product): Observable<any>
  {
    return this.http.put<any>(`${this.apiUrl}/${product.productId}`, product);
  }

  DeleteProduct(productId: string): Observable<any>
  {
    return this.http.delete<any>(`${this.apiUrl}/${productId}`);
  }

  GetAllProductsThatStartWithLetter(letter: string): Observable<Product[]>
  {
    return this.http.get<Product[]>(`${this.apiUrl}/name`, {params: {letter}});
  }

  DecreaseQuantity(productId: string): Observable<any>
  {
    return this.http.get(`${this.apiUrl}/${productId}/sold`);
  }

  SetAvgRatingOfProduct(productId: string): Observable<any>
  {
    return this.http.get(`${this.apiUrl}/${productId}/rating`);
  }

}
