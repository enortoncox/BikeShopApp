import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map, of, switchMap } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { AuthenticationResponse } from '../interfaces/authenticationResponse';
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

      //If no tokens are found just return true.
      if(localStorage["token"] == undefined){
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
        //If Jwt token is invalid but refresh is still valid then generate new tokens.
        else{
          return this.authService.GetNewJwtToken().pipe(map((response: AuthenticationResponse) => 
          { 
              this.authService.SetCurrentUser(response.user, response.isAdmin);
              localStorage["token"] = response.token;
              localStorage["refreshToken"] = response.refreshToken;
              return true;
          }))
        }
      }));
  }
}