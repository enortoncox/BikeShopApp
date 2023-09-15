import { ErrorHandler, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { HomeComponent } from './components/home/home.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorComponent } from './components/error/error.component';
import { UnauthorisedComponent } from './components/unauthorised/unauthorised.component';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { UtilitiesModule } from './components/utilities/utilities.module';
import { CustomErrorHandler } from './services/custom-error-handler.service';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { IsAdminGuard } from './routeGuards/is-admin.guard';
import { IsLoggedInGuard } from './routeGuards/is-logged-in.guard';
import { JwtLoginGuard } from './routeGuards/jwt-login.guard';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HomeComponent,
    ErrorComponent,
    UnauthorisedComponent,
  ],
  imports: [
    BrowserModule,
    UtilitiesModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: ErrorHandler, useClass: CustomErrorHandler},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
