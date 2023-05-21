import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Category } from "../interfaces/category";
import { CategoriesService } from "../services/categories.service";

@Injectable({
    providedIn: "root"
})
export class CategoriesResolver implements Resolve<Category[]>{
    constructor(private categoriesService: CategoriesService){}

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Category[] | Observable<Category[]> | Promise<Category[]> {
        return this.categoriesService.GetCategories();
    }

}