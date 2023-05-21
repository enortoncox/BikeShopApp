import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { User } from "../interfaces/user";
import { UsersService } from "../services/users.service";

@Injectable({
    providedIn: "root"
})
export class UserResolver implements Resolve<User>
{
    constructor(private usersService: UsersService) {}

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): User | Observable<User> | Promise<User> {
        return this.usersService.GetUser(route.params['userId']);
    }
}