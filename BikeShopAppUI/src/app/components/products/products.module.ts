import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsListComponent } from './products-list/products-list.component';
import { ProductsRoutingModule } from './products-routing.module';
import { UtilitiesModule } from '../utilities/utilities.module';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductCreateFormComponent } from './product-create-form/product-create-form.component';
import { ProductUpdateFormComponent } from './product-update-form/product-update-form.component';
import { IsAdminGuard } from 'src/app/routeGuards/is-admin.guard';
import { IsLoggedInGuard } from 'src/app/routeGuards/is-logged-in.guard';
import { JwtLoginGuard } from 'src/app/routeGuards/jwt-login.guard';

@NgModule({
  declarations: [
    ProductsListComponent,
    ProductDetailComponent,
    ProductCreateFormComponent,
    ProductUpdateFormComponent
  ],
  imports: [
    CommonModule,
    UtilitiesModule,
    FormsModule,
    ReactiveFormsModule,
    ProductsRoutingModule
  ],
  providers: [IsAdminGuard, IsLoggedInGuard, JwtLoginGuard]
})
export class ProductsModule { }
