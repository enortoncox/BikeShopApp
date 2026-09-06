import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { AsyncGuard } from '../interfaces/asyncGuard';

@Injectable({
  providedIn: 'root'
})
export class IsAdminGuard implements AsyncGuard {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

      return this.isLoggedInAdmin();
  }

  isLoggedInAdmin(): Observable<boolean>{
    if(this.authService.isLoggedInAdmin)
    {
      return of(true);
    }
    else{
      this.router.navigate(['/unauthorised']);
      return of(false);
    }
  }
}
