import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ReviewResolver } from "src/app/resolvers/review.resolver";
import { UserResolver } from "src/app/resolvers/user.resolver";
import { FormLeaveGuard } from "src/app/routeGuards/form-leave.guard";
import { IsLoggedInGuard } from "src/app/routeGuards/is-logged-in.guard";
import { ReviewCreateFormComponent } from "./review-create-form/review-create-form.component";
import { ReviewUpdateFormComponent } from "./review-update-form/review-update-form.component";
import { UserAccountPublicComponent } from "./user-account-public/user-account-public.component";
import { JwtLoginGuard } from "src/app/routeGuards/jwt-login.guard";
import { orderedAsyncGuards } from "src/app/functions/ordered-async-guards";

const routes: Routes = [
    {path: ":productId/new", component: ReviewCreateFormComponent, canDeactivate: [FormLeaveGuard], canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard])]},
    {path: ":productId/update/:reviewId", component: ReviewUpdateFormComponent, resolve: {reviewData: ReviewResolver}, canDeactivate: [FormLeaveGuard], canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard])]},
    {path: "user/:userId", component: UserAccountPublicComponent, resolve: {userData: UserResolver}, canActivate: [JwtLoginGuard]},
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ReviewsRoutingModule{}