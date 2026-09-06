import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { Review } from "../interfaces/review";
import { ReviewsService } from "../services/reviews.service";

@Injectable({
    providedIn: "root"
})
export class ReviewResolver 
{

    constructor(private reviewsService: ReviewsService) {}

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.reviewsService.GetReview(route.params['reviewId']);
    }

}