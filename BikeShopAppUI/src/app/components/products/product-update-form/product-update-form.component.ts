import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Category } from 'src/app/interfaces/category';
import { Product } from 'src/app/interfaces/product';
import { CategoriesService } from 'src/app/services/categories.service';
import { FilesService } from 'src/app/services/files.service';
import { ProductsService } from 'src/app/services/products.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-product-update-form',
  templateUrl: './product-update-form.component.html',
  styleUrls: ['./product-update-form.component.css']
})
export class ProductUpdateFormComponent implements OnInit{
  productUpdateForm!: FormGroup;
  product: Product;
  fileName: string = "";
  dbPath: string = "";
  isSubmitting: boolean;
  categories: Category[] = [];
  orginalFilePath: string = "";
  newFilePath: string = "";
  domain: string = "";
  noImage: string = "";

  constructor(private productsService: ProductsService, private router: Router, private filesService: FilesService, private categoriesService: CategoriesService, private route: ActivatedRoute)
  {
    this.product = this.route.snapshot.data['productData'];
    this.dbPath = this.product.imgPath;
    this.fileName = this.dbPath.split("\\")[2];
    this.isSubmitting = false;
    this.domain = environment.domain;
    this.noImage= environment.noImage;
  }

  canExit()
  {
    if((!this.productUpdateForm.pristine) && this.isSubmitting == false)
    {
      if(confirm("You have unsaved changes. Are you sure you want to leave?"))
      {
        if(this.newFilePath != "")
        {
          this.filesService.DeleteFile(this.newFilePath).subscribe(() => 
          {
            this.product.imgPath = this.orginalFilePath;
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
    this.categoriesService.GetCategories().subscribe((categoriesData: Category[]) => 
    {
      this.categories = categoriesData;
    })

    this.productUpdateForm = new FormGroup({
      name: new FormControl(this.product.name, Validators.required),
      price: new FormControl(this.product.price, Validators.required),
      quantity: new FormControl(this.product.quantity, Validators.required),
      categoryId: new FormControl(this.product.categoryId, Validators.required),
    });

    this.orginalFilePath = this.product.imgPath;
  }

  onSubmit()
  {
    this.product.name = this.productUpdateForm.value.name;
    this.product.price = this.productUpdateForm.value.price;
    this.product.quantity = this.productUpdateForm.value.quantity;
    this.product.categoryId = this.productUpdateForm.value.categoryId;

    this.productsService.UpdateProduct(this.product).subscribe(() => 
    {
      if(this.newFilePath != "")
      {
        this.filesService.DeleteFile(this.orginalFilePath).subscribe(() => {
          this.isSubmitting = true;
          this.router.navigate(['/home']);
        })
      }
      else{
        this.isSubmitting = true;
        this.router.navigate(['/home']);
      }
    });
  }

  UploadImage(files: any)
  {
      if(files.length == 0)
      {
        return;
      }

      if(this.newFilePath != ""){
        this.filesService.DeleteFile(this.newFilePath).subscribe(() => {});
      }

      let fileToUpload = files[0];
      this.fileName = fileToUpload.name;

      let formData = new FormData();
      formData.append("file", fileToUpload, fileToUpload.name);

      this.filesService.UploadFile(formData).subscribe((res: any) => 
      {
        this.dbPath = res.dbPath;
        this.product.imgPath = this.dbPath;
        this.newFilePath = this.dbPath;
        this.productUpdateForm.markAsDirty();
      })
  }

  noImageFound(e: any){
    e.target.src = this.noImage;
  }
}
