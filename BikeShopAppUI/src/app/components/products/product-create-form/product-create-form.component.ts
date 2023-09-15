import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Category } from 'src/app/interfaces/category';
import { Product } from 'src/app/interfaces/product';
import { IDeactivateComponent } from 'src/app/routeGuards/form-leave.guard';
import { CategoriesService } from 'src/app/services/categories.service';
import { FilesService } from 'src/app/services/files.service';
import { ProductsService } from 'src/app/services/products.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-product-create-form',
  templateUrl: './product-create-form.component.html',
  styleUrls: ['./product-create-form.component.css']
})
export class ProductCreateFormComponent implements OnInit, IDeactivateComponent{
  productCreateForm!: FormGroup;
  product: Product = {} as Product;
  fileName: string = "";
  dbPath: string = "";
  isSubmitting: boolean;
  categories: Category[] = [];
  domain: string = "";
  noImage: string = "";

  constructor(private productsService: ProductsService, private router: Router, private filesService: FilesService, private categoriesService: CategoriesService)
  {
    this.isSubmitting = false;
    this.domain = environment.domain;
    this.noImage = environment.noImage;
  }

  canExit()
  {
    if((!this.productCreateForm.pristine || this.fileName != "") && this.isSubmitting == false)
    {
      if(confirm("You have unsaved changes. Are you sure you want to leave?"))
      {
        if(this.dbPath != "")
        {
          this.filesService.DeleteFile(this.dbPath).subscribe(() => 
          {
            return true;
          });
        }
      }
      else
      {
        return false;
      }
    }
    return true;
  }

  ngOnInit(): void {
    this.productCreateForm = new FormGroup({
      name: new FormControl("", Validators.required),
      price: new FormControl("", Validators.required),
      quantity: new FormControl("", Validators.required),
      categoryId: new FormControl("", Validators.required),
    });

    this.categoriesService.GetCategories().subscribe((categoriesData: Category[]) => 
    {
      this.categories = categoriesData;
    })
  }

  onSubmit()
  {
    this.product.name = this.productCreateForm.value.name;
    this.product.price = this.productCreateForm.value.price;
    this.product.quantity = this.productCreateForm.value.quantity;
    this.product.categoryId = this.productCreateForm.value.categoryId;

    this.productsService.CreateProduct(this.product).subscribe(() => 
    {
      this.isSubmitting = true;
      this.router.navigate(['/home']);
    });
  }

  UploadImage(files: any)
  {
      if(files.length == 0)
      {
        return;
      }

      let fileToUpload = files[0];
      this.fileName = fileToUpload.name;

      let formData = new FormData();
      formData.append("file", fileToUpload, fileToUpload.name);

      this.filesService.UploadFile(formData).subscribe((res: any) => 
      {
        this.dbPath = res.dbPath;
        this.product.imgPath = this.dbPath;
        this.productCreateForm.markAsDirty();
      })
  }

  noImageFound(e: any){
    e.target.src = this.noImage;
  }
}
