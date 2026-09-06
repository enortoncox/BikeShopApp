import { Injectable } from '@angular/core';
import {
  HttpErrorResponse,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import {
  BehaviorSubject,
  Observable,
  catchError,
  filter,
  finalize,
  switchMap,
  take,
  throwError
} from 'rxjs';

import { AuthService } from '../services/auth.service';
import { AuthenticationResponse } from '../interfaces/authenticationResponse';
import { CustomError } from '../interfaces/customError';
import { ErrorService } from '../services/error.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  private isRefreshing = false;

  private refreshTokenSubject =
    new BehaviorSubject<string | null>(null);

  constructor(
    private authService: AuthService,
    private errorService: ErrorService
  ) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {

    return next.handle(request).pipe(

      catchError((error: HttpErrorResponse) => {

        if (error.status !== 401) {
          this.errorService.SetCurrentError(
            error.error as CustomError
          );

          return throwError(() => error);
        }

        if (
          request.url.includes('/generate-new-jwt-token') ||
          request.url.includes('/login')
        ) {
          this.errorService.SetCurrentError(
            error.error as CustomError
          );

          return throwError(() => error);
        }

        const token = localStorage.getItem('token');
        const refreshToken = localStorage.getItem('refreshToken');

        if (!token || !refreshToken) {
          this.authService.Logout('/auth/login');
          return throwError(() => error);
        }

        // Another request is already refreshing the token.
        if (this.isRefreshing) {

          return this.refreshTokenSubject.pipe(
            filter(newToken => newToken !== null),
            take(1),
            switchMap(newToken => {

              const retryRequest = request.clone({
                setHeaders: {
                  Authorization: `Bearer ${newToken}`
                }
              });

              return next.handle(retryRequest);
            })
          );
        }

        // This request becomes responsible for refreshing the token.
        this.isRefreshing = true;
        this.refreshTokenSubject.next(null);

        return this.authService.GetNewJwtToken().pipe(

          switchMap((response: AuthenticationResponse) => {

            if (!response.token) {
              return throwError(
                () => new Error("No JWT token was returned.")
              );
            }

            localStorage.setItem('token', response.token);

            if (response.refreshToken) {
              localStorage.setItem(
                'refreshToken',
                response.refreshToken
              );
            }

            this.authService.SetCurrentUser(
              response.user,
              response.isAdmin
            );

            this.refreshTokenSubject.next(response.token);

            const retryRequest = request.clone({
              setHeaders: {
                Authorization: `Bearer ${response.token}`
              }
            });

            return next.handle(retryRequest);
          }),

          catchError((refreshError) => {

            this.refreshTokenSubject.next(null);

            this.authService.Logout('/auth/login');

            return throwError(() => refreshError);
          }),

          finalize(() => {
            this.isRefreshing = false;
          })
        );
      })
    );
  }
}