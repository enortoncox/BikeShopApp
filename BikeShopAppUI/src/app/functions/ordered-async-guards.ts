import { CanActivateFn, Router } from "@angular/router";
import { concatMap, from, last, takeWhile } from "rxjs";
import { AsyncGuard } from "../interfaces/asyncGuard";
import {inject} from '@angular/core';
import { AuthService } from "../services/auth.service";

//Make the Route Guards run in order, and wait for asynchronous code to finish before moving on. 
export function orderedAsyncGuards(
    guards: Array<new (authService: AuthService, router: Router) => AsyncGuard>
  ): CanActivateFn {
    return (route, state) => {

      const guardInstances = guards.map(inject) as AsyncGuard[];

      return from(guardInstances).pipe(
        concatMap((guard) => guard.canActivate(route, state)),
        takeWhile((value) => value === true, true),
        last()
      );
    };
  }