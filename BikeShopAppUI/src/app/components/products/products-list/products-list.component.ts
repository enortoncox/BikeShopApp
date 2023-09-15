import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Category } from 'src/app/interfaces/category';
import { Product } from 'src/app/interfaces/product';
import { ProductsResponse } from 'src/app/interfaces/productsResponse';
import { CategoriesService } from 'src/app/services/categories.service';
import { ProductsService } from 'src/app/services/products.service';
import { ReviewsService } from 'src/app/services/reviews.service';

@Component({
  selector: 'app-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.css']
})
export class ProductsListComponent implements OnInit{

  products: Product[];
  categoryId: string = "";
  categoryName: string = "";
  currentPage: number = 1;
  pageNumbers: Number[] = [];
  pageResults: number = 4;
  isFiltered: boolean = false;
  rating: string = "";
  price: string = "";

  constructor(private productsService: ProductsService, private route: ActivatedRoute, private categoriesService: CategoriesService)
  {
    this.products = this.route.snapshot.data['productsData'];
  }
  
  ngOnInit(): void {
    this.route.queryParams.subscribe(paramData => { 

      this.categoryId = paramData['category']; 
      this.currentPage = 1;
      
      this.productsService.GetProductsByPage(this.categoryId, this.currentPage, this.pageResults).subscribe((productsResponse: ProductsResponse) => 
      {
          this.products = productsResponse.products;
          this.pageNumbers = [...Array(productsResponse.pages).keys()].map(el => el + 1);
          this.isFiltered = false;
      });

      this.categoriesService.GetCategory(this.categoryId).subscribe((category: Category) => this.categoryName = category.name);
    });
  }

  filterProducts(priceValue: string, ratingValue: string)
  {
    this.rating = ratingValue;
    this.price = priceValue;
    this.currentPage = 1;

    this.productsService.GetFilteredProductsByPage(this.categoryId, this.currentPage, this.pageResults, this.price, this.rating).subscribe((productsResponse: ProductsResponse) => 
    {
        this.products = productsResponse.products;
        this.pageNumbers = [...Array(productsResponse.pages).keys()].map(el => el + 1);
        this.isFiltered = true;
    });
  }

  changePage(page: any)
  {
    this.currentPage = page;

    if(this.isFiltered)
    {
      this.productsService.GetFilteredProductsByPage(this.categoryId, this.currentPage, this.pageResults, this.price, this.rating).subscribe((productsResponse: ProductsResponse) => 
      {
          this.products = productsResponse.products;
          this.isFiltered = true;
      });
    }
    else
    {
      this.productsService.GetProductsByPage(this.categoryId, page, this.pageResults).subscribe((productsResponse: ProductsResponse) => {
        this.products = productsResponse.products;
        this.isFiltered = false;
      });
  }
  }
}
