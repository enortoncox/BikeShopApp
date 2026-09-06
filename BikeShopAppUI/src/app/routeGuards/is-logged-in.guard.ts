import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable, of } from "rxjs";
import { AuthService } from "../services/auth.service";
import { AsyncGuard } from "../interfaces/asyncGuard";

@Injectable({
    providedIn: "root"
})
export class IsLoggedInGuard implements AsyncGuard
{
    constructor(private authService: AuthService, private router: Router) {}
    
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        return this.isLoggedIn();
    }

    isLoggedIn(): Observable<boolean>{

        if(this.authService.isLoggedIn)
        {
            return of(true);
        }
        else{        
            this.router.navigate(['/auth/login']);
            return of(false);
        }
    }
}