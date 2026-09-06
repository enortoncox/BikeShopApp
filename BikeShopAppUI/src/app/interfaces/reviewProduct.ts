import { Product } from "./product";
import { Review } from "./review";

export interface ReviewProduct{
    review: Review,
    product: Product
}