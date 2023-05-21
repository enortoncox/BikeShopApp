import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  currentError: ReplaySubject<string> = new ReplaySubject(1);

  constructor(){}

  SetCurrentError(error: string)
  {
    this.currentError.next(error);
  }

  GetCurrentError(): Observable<any>
  {
    return this.currentError.asObservable();
  }
}
