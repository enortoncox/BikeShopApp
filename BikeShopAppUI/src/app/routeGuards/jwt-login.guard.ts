import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map, of, switchMap } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { AsyncGuard } from '../interfaces/asyncGuard';
import { VerifyTokenResponse } from '../interfaces/verifyTokenResponse';

@Injectable({
  providedIn: 'root'
})
export class JwtLoginGuard implements AsyncGuard {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree>{

      const token = localStorage.getItem("token");

      //If no tokens are found just return true.
      if(!token){
        return of(true);
      }


      return this.authService.VerifyJwtToken().pipe(switchMap((response: VerifyTokenResponse) => {

        //If tokens aren't valid then remove them from local storage.
        if(response.user == null || response.refreshValid == false){
          localStorage.removeItem("token");
          localStorage.removeItem("refreshToken");
          return of(true);
        }
        //If the Jwt token is still valid then set the current user.
        else if(response.jwtValid == true){
          this.authService.SetCurrentUser(response.user, response.isAdmin);
          return of(true);
        }
        else {
          this.authService.SetCurrentUser(response.user, response.isAdmin);
          return of(true);
        }
      }));
  }
}