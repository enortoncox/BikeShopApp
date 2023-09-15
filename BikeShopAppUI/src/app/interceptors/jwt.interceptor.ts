import { Injectable } from '@angular/core';
import { HttpRequest,HttpHandler,HttpEvent,HttpInterceptor,HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';

//Add an Authorization header with the Jwt Token for each request.
@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    var token = localStorage["token"];
    var headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${token}`);
    
    var req = request.clone({
      headers: headers
    });
      
    return next.handle(req);
  }
}
