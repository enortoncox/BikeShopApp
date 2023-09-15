import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Product } from "../interfaces/product";
import { ProductsService } from "../services/products.service";

@Injectable({
    providedIn: "root"
})
export class ProductResolver {
    constructor(private productsService: ProductsService){}

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Product | Observable<Product> | Promise<Product> {
        return this.productsService.GetProduct(route.params['productId']);
    }


}