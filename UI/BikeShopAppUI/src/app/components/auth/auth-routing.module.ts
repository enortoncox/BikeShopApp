import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { FormLeaveGuard } from "src/app/routeGuards/form-leave.guard";
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";

const routes: Routes = [
    {path: "login", component: LoginComponent},
    {path: "register", component: RegisterComponent, canDeactivate: [FormLeaveGuard]},
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AuthRoutingModule {}