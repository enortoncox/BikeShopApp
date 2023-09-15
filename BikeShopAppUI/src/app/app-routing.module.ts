import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ErrorComponent } from "./components/error/error.component";
import { HomeComponent } from "./components/home/home.component";
import { UnauthorisedComponent } from "./components/unauthorised/unauthorised.component";
import { CategoriesResolver } from "./resolvers/categories.resolver";
import { JwtLoginGuard } from "./routeGuards/jwt-login.guard";

const routes: Routes = [
    {path: "", redirectTo: "home", pathMatch: "full"},
    {path: "home", title: "Mega Cycles", component: HomeComponent, resolve: {categoriesData: CategoriesResolver}, canActivate: [JwtLoginGuard]},
    {path: "products", title: "Mega Cycles", loadChildren: () => import("./components/products/products.module").then(m => m.ProductsModule)},
    {path: "reviews", title: "Mega Cycles", loadChildren: () => import("./components/reviews/reviews.module").then(m => m.ReviewsModule)},
    {path: "auth", title: "Mega Cycles", loadChildren: () => import("./components/auth/auth.module").then(m => m.AuthModule)},
    {path: "user", title: "Mega Cycles", loadChildren: () => import("./components/user/user.module").then(m => m.UserModule)},
    {path: "admin", title: "Mega Cycles", loadChildren: () => import("./components/admin/admin.module").then(m => m.AdminModule)},
    {path: "unauthorised", title: "Mega Cycles", component: UnauthorisedComponent, canActivate: [JwtLoginGuard]},
    {path: "**", title: "Mega Cycles", component: ErrorComponent, canActivate: [JwtLoginGuard]},
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule{}