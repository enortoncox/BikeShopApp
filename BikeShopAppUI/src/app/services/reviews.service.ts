import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Review } from "../interfaces/review";
import { environment } from "src/environments/environment.development";

@Injectable({
    providedIn: 'root'
})
export class ReviewsService
{
    apiUrl = environment.domain + "api/reviews";

    constructor(private http: HttpClient) {}

    GetReview(reviewId: string): Observable<Review>
    {
        return this.http.get<Review>(`${this.apiUrl}/${reviewId}`);
    }

    GetReviews(): Observable<Review>
    {
        return this.http.get<Review>(this.apiUrl);
    }

    GetReviewsOfProduct(productId: string): Observable<Review[]>
    {
        return this.http.get<Review[]>(`${this.apiUrl}/products/${productId}`);
    }

    CreateReview(review: Review): Observable<any>
    {
        return this.http.post(this.apiUrl, review);
    }

    UpdateReview(review: Review): Observable<any>
    {
        return this.http.put(`${this.apiUrl}/${review.reviewId}`, review);
    }

    DeleteReview(reviewId: string): Observable<any>
    {
        return this.http.delete(`${this.apiUrl}/${reviewId}`);
    }

    GetAllReviewsFromAUser(userId: string): Observable<Review[]>
    {
        return this.http.get<Review[]>(`${this.apiUrl}/users/${userId}`);
    }
} 