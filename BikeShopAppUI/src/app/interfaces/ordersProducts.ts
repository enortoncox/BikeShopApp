import { Product } from "./product";

export interface OrdersProducts
{
    orderId: string,
    products: Product[]
}