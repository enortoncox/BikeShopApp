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
import { CustomErrorHandler } from './custom-error-handler.service';

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
    {provide: ErrorHandler, useClass: CustomErrorHandler},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
