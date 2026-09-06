import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthUserResolver } from "src/app/resolvers/auth-user.resolver";
import { CartResolver } from "src/app/resolvers/cart.resolver";
import { FormLeaveGuard } from "src/app/routeGuards/form-leave.guard";
import { IsLoggedInGuard } from "src/app/routeGuards/is-logged-in.guard";
import { UserAccountPrivateComponent } from "./user-account-private/user-account-private.component";
import { UserCartComponent } from "./user-cart/user-cart.component";
import { UserCheckoutComponent } from "./user-checkout/user-checkout.component";
import { UserOrdersComponent } from "./user-orders/user-orders.component";
import { UserPasswordFormComponent } from "./user-password-form/user-password-form.component";
import { UserPaymentComponent } from "./user-payment/user-payment.component";
import { UserUpdateFormComponent } from "./user-update-form/user-update-form.component";
import { JwtLoginGuard } from "src/app/routeGuards/jwt-login.guard";
import { orderedAsyncGuards } from "src/app/functions/ordered-async-guards";

const routes: Routes = [
    {path: "cart", component: UserCartComponent, resolve: {currentUser: AuthUserResolver , userCart: CartResolver}, canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard])]},
    {path: "checkout", component: UserCheckoutComponent, resolve: {currentUser: AuthUserResolver, userCart: CartResolver}, canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard])]},
    {path: "checkout/payment", component: UserPaymentComponent, canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard])]},
    {path: "orders", component: UserOrdersComponent, resolve: {currentUser: AuthUserResolver}, canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard])]},
    {path: "account", component: UserAccountPrivateComponent, resolve: {currentUser: AuthUserResolver}, canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard])]},
    {path: "account/update", component: UserUpdateFormComponent, resolve: {currentUser: AuthUserResolver}, canDeactivate: [FormLeaveGuard], canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard])]},
    {path: "account/update/password", component: UserPasswordFormComponent, resolve: {currentUser: AuthUserResolver}, canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard])]},
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UserRoutingModule {}