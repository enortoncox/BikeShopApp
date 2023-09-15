import { ErrorHandler, Injectable, NgZone } from "@angular/core";
import { ErrorService } from "./error.service";
import { HttpErrorResponse } from "@angular/common/http";
import { CustomError } from "../interfaces/customError";
import { AuthService } from "./auth.service";
import { AuthenticationResponse } from "../interfaces/authenticationResponse";
import { map } from "rxjs";
import { VerifyTokenResponse } from "../interfaces/verifyTokenResponse";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class CustomErrorHandler implements ErrorHandler
{
    constructor(private errorService: ErrorService, private authService: AuthService, private router: Router, private zone: NgZone){}

    handleError(error: HttpErrorResponse | any): void {

      console.log(error);

        //Solves "Error: Uncaught (in promise):"
        if (error.promise && error.rejection) {
            // Promise rejection wrapped by zone.js
            error = error.rejection;
        }

        //Problem Response
        if(error.error?.title != null)
        {
          let customError: CustomError = {
            status: error.error.status,
            detail: error.error.detail,
            title: error.error.title,
            errors: error.error.errors,
          };
          
          if (error.error.status == 401) {

            if (
              localStorage['token'] != undefined &&
              localStorage['refreshToken'] != undefined
            ) {
              this.VerifyAndGenerateJwtToken(customError);
            } 
            //No tokens
            else {
              this.errorService.SetCurrentError(customError);
              this.zone.run(() => {
                this.router.navigate(['/auth/login']);
              });
            }
          }
          //error.error.status is not 401 Unauthorized
          else{
            this.errorService.SetCurrentError(customError);
          }
        }

        //error.error.title is null
        else
        {
            let customError: CustomError =
            {
                status: error.status,
                detail: error.message,
                title: "Error",
                errors: null
            }

            switch(error.status)
            {
                case 0:
                    customError.detail = "Could not connect to the database."
                    break;
                case 400:
                    customError.detail = "There was an issue with your request.";
                    break;
                case 401:
                  console.log("Dealing with Standard 401 error!");
                    if (
                        localStorage['token'] != undefined &&
                        localStorage['refreshToken'] != undefined
                      ){
                        this.VerifyAndGenerateJwtToken(customError);
                    }
                    else{
                        customError.detail = "You are not authenticated. Please log in.";
                        this.errorService.SetCurrentError(customError);
                        this.zone.run(() => {
                          this.router.navigate(['/auth/login']);
                        })
                    }

                    break;
                case 403:
                    customError.detail = "You are not authorized to access this resource";
                    break;
                case 404:
                      customError.detail = "The resource you are looking for wasn't found.";
                      break;
                case 500:
                    customError.detail = "Internal Server Error";
                    break;
    
            }

            if(error.status != "401")
            {
                this.errorService.SetCurrentError(customError);
            }
        }
    }

    VerifyAndGenerateJwtToken(customError: CustomError){
      this.authService.VerifyJwtToken().subscribe((response: VerifyTokenResponse) => {
        if(response.user == null || response.refreshValid == false){
          if(customError.detail == undefined)
          {
            customError.detail = "You are not authenticated. Please log in.";
          }
          this.errorService.SetCurrentError(customError);

          this.zone.run(() => {
          this.authService.Logout("/auth/login");
          })    
        }
        else if(response.jwtValid == true){
          this.authService.SetCurrentUser(response.user, response.isAdmin);
        }
        else{       
          this.authService.GetNewJwtToken().pipe(map((response: AuthenticationResponse) => 
          { 
            localStorage["token"] = response.token;
            localStorage["refreshToken"] = response.refreshToken;
            this.authService.SetCurrentUser(response.user, response.isAdmin);          
          })).subscribe();
        }
      });
    }
}