import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../interfaces/category';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  apiURL = "https://localhost:7000/api/categories"

  constructor(private http: HttpClient) { }

  GetCategory(categoryId: string): Observable<any>
  {
    return this.http.get<Category>(`${this.apiURL}/${categoryId}`);
  }

  GetCategories(): Observable<Category[]>
  {
    return this.http.get<Category[]>(`${this.apiURL}`);
  }
}
