import { Product } from "./product";

export interface ProductsResponse
{
    products: Product[],
    currentPage: number,
    pages: number
}