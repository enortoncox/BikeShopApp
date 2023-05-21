import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { UserResolver } from "src/app/resolvers/user.resolver";
import { FormLeaveGuard } from "src/app/routeGuards/form-leave.guard";
import { IsAdminGuard } from "src/app/routeGuards/is-admin.guard";
import { LoggedInGuard } from "src/app/routeGuards/logged-in.guard";
import { ControlComponent } from "./control/control.component";
import { UserUpdateAdminFormComponent } from "./user-update-admin-form/user-update-admin-form.component";

const routes: Routes = [
    {path: "control", component: ControlComponent, canActivate: [LoggedInGuard, IsAdminGuard]},
    {path: "user/:userId/update", component: UserUpdateAdminFormComponent, resolve: {userData: UserResolver}, canActivate: [LoggedInGuard, IsAdminGuard], canDeactivate: [FormLeaveGuard]},
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminRoutingModule{}