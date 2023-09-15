import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { CustomError } from '../interfaces/customError';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  currentError: ReplaySubject<CustomError> = new ReplaySubject(1);

  constructor(){}

  SetCurrentError(error: CustomError)
  {
    this.currentError.next(error);
  }

  GetCurrentError(): Observable<CustomError>
  {
    return this.currentError.asObservable();
  }
}
