import { Component, Input } from '@angular/core';
import { ProductItem } from 'src/app/interfaces/productItem';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.css']
})
export class ProductItemComponent {
  @Input() itemInfo: ProductItem = {} as ProductItem;
  domain: string = "";
  noImage: string = "";

  constructor(){
    this.domain = environment.domain;
    this.noImage = environment.noImage;
  }

  noImageFound(e: any){
    e.target.src = this.noImage;
  }
}
