import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../interfaces/category';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  apiURL = environment.domain + "api/categories"

  constructor(private http: HttpClient) { }

  GetCategory(categoryId: string = "1"): Observable<Category>
  {
    return this.http.get<Category>(`${this.apiURL}/${categoryId}`);
  }

  GetCategories(): Observable<Category[]>
  {
    return this.http.get<Category[]>(`${this.apiURL}`);
  }
}
