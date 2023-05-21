import { ErrorHandler, Injectable } from "@angular/core";
import { ErrorService } from "./services/error.service";

@Injectable({
    providedIn: "root"
})
export class CustomErrorHandler implements ErrorHandler
{
    constructor(private errorService: ErrorService){}

    handleError(error: any): void {

        if(error.error == null)
        {
            this.errorService.SetCurrentError(error.message);
        }
        else{
            this.errorService.SetCurrentError(error.error.toString());
        }
    }
}