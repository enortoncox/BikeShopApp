import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { FormLeaveGuard } from "src/app/routeGuards/form-leave.guard";
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";
import { JwtLoginGuard } from "src/app/routeGuards/jwt-login.guard";

const routes: Routes = [
    {path: "login", component: LoginComponent, canActivate: [JwtLoginGuard]},
    {path: "register", component: RegisterComponent, canDeactivate: [FormLeaveGuard], canActivate: [JwtLoginGuard]},
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AuthRoutingModule {}