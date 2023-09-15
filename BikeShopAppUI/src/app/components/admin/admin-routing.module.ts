import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { UserResolver } from "src/app/resolvers/user.resolver";
import { FormLeaveGuard } from "src/app/routeGuards/form-leave.guard";
import { IsAdminGuard } from "src/app/routeGuards/is-admin.guard";
import { IsLoggedInGuard } from "src/app/routeGuards/is-logged-in.guard";
import { ControlComponent } from "./control/control.component";
import { UserUpdateAdminFormComponent } from "./user-update-admin-form/user-update-admin-form.component";
import { JwtLoginGuard } from "src/app/routeGuards/jwt-login.guard";
import { orderedAsyncGuards } from "src/app/functions/ordered-async-guards";

const routes: Routes = [
    {path: "control", canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard, IsAdminGuard])], component: ControlComponent},
    {path: "user/:userId/update", component: UserUpdateAdminFormComponent, resolve: {userData: UserResolver}, canActivate: [orderedAsyncGuards([JwtLoginGuard, IsLoggedInGuard, IsAdminGuard])], canDeactivate: [FormLeaveGuard]},
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminRoutingModule{}