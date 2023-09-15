import { Order } from "./order";

export interface OrdersResponse{
    orders: Order[],
    currentPage: number,
    pages: number
}