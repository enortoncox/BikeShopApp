import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthUserResolver } from "src/app/resolvers/auth-user.resolver";
import { ReviewResolver } from "src/app/resolvers/review.resolver";
import { UserResolver } from "src/app/resolvers/user.resolver";
import { FormLeaveGuard } from "src/app/routeGuards/form-leave.guard";
import { LoggedInGuard } from "src/app/routeGuards/logged-in.guard";
import { ReviewCreateFormComponent } from "./review-create-form/review-create-form.component";
import { ReviewUpdateFormComponent } from "./review-update-form/review-update-form.component";
import { UserAccountPublicComponent } from "./user-account-public/user-account-public.component";

const routes: Routes = [
    {path: ":productId/new", component: ReviewCreateFormComponent, canDeactivate: [FormLeaveGuard], canActivate: [LoggedInGuard]},
    {path: ":productId/update/:reviewId", component: ReviewUpdateFormComponent, resolve: {reviewData: ReviewResolver}, canDeactivate: [FormLeaveGuard], canActivate: [LoggedInGuard]},
    {path: "user/:userId", component: UserAccountPublicComponent, resolve: {userData: UserResolver}},
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ReviewsRoutingModule{}