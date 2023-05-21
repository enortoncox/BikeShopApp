import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ProductResolver } from "src/app/resolvers/product.resolver";
import { ProductsResolver } from "src/app/resolvers/products.resolver";
import { FormLeaveGuard } from "src/app/routeGuards/form-leave.guard";
import { IsAdminGuard } from "src/app/routeGuards/is-admin.guard";
import { LoggedInGuard } from "src/app/routeGuards/logged-in.guard";
import { ProductCreateFormComponent } from "./product-create-form/product-create-form.component";
import { ProductDetailComponent } from "./product-detail/product-detail.component";
import { ProductUpdateFormComponent } from "./product-update-form/product-update-form.component";
import { ProductsListComponent } from "./products-list/products-list.component";

const routes: Routes = [
    {path: "", component: ProductsListComponent, resolve: {productsData: ProductsResolver}},
    {path: "new", component: ProductCreateFormComponent, canDeactivate: [FormLeaveGuard], canActivate: [LoggedInGuard, IsAdminGuard]},
    {path: ":productId/update", component: ProductUpdateFormComponent, canDeactivate: [FormLeaveGuard], resolve: {productData: ProductResolver}, canActivate: [LoggedInGuard, IsAdminGuard]},
    {path: ":productId", component: ProductDetailComponent, resolve: {productData: ProductResolver}},

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProductsRoutingModule{}