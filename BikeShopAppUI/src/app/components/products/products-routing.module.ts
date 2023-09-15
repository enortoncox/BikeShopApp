import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ProductResolver } from "src/app/resolvers/product.resolver";
import { ProductsResolver } from "src/app/resolvers/products.resolver";
import { FormLeaveGuard } from "src/app/routeGuards/form-leave.guard";
import { IsAdminGuard } from "src/app/routeGuards/is-admin.guard";
import { IsLoggedInGuard } from "src/app/routeGuards/is-logged-in.guard";
import { ProductCreateFormComponent } from "./product-create-form/product-create-form.component";
import { ProductDetailComponent } from "./product-detail/product-detail.component";
import { ProductUpdateFormComponent } from "./product-update-form/product-update-form.component";
import { ProductsListComponent } from "./products-list/products-list.component";
import { JwtLoginGuard } from "src/app/routeGuards/jwt-login.guard";
import { orderedAsyncGuards } from "src/app/functions/ordered-async-guards";

const routes: Routes = [
    {path: "", component: ProductsListComponent, resolve: {productsData: ProductsResolver}, canActivate: [JwtLoginGuard]},
    {path: "new", component: ProductCreateFormComponent, canDeactivate: [FormLeaveGuard], canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard, IsAdminGuard])]},
    {path: ":productId/update", component: ProductUpdateFormComponent, canDeactivate: [FormLeaveGuard], resolve: {productData: ProductResolver}, canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard, IsAdminGuard])]},
    {path: ":productId", component: ProductDetailComponent, resolve: {productData: ProductResolver}, canActivate: [JwtLoginGuard]},

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProductsRoutingModule{}