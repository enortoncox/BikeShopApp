import { bootstrapApplication } from '@angular/platform-browser';
import { ErrorHandler, provideZoneChangeDetection } from '@angular/core';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi, withXhr} from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { AppComponent } from './app/app.component';
import { routes } from './app/app-routing.module';
import { ErrorInterceptor } from './app/interceptors/error.interceptor';
import { JwtInterceptor } from './app/interceptors/jwt.interceptor';
import { CustomErrorHandler } from './app/services/custom-error-handler.service';
import { importProvidersFrom } from '@angular/core';
import { UtilitiesModule } from './app/components/utilities/utilities.module';

bootstrapApplication(AppComponent, {
  providers: [
    provideZoneChangeDetection(),

    provideRouter(routes),

    importProvidersFrom(UtilitiesModule),

    provideHttpClient(
      withXhr(),
      withInterceptorsFromDi()
    ),

    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    },

    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    },

    {
      provide: ErrorHandler,
      useClass: CustomErrorHandler
    }
  ]
})
.catch(err => console.error(err));