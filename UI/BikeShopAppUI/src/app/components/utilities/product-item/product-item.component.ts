import { Component, Input } from '@angular/core';
import { ProductItem } from 'src/app/interfaces/productItem';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.css']
})
export class ProductItemComponent {
  @Input() itemInfo: ProductItem = {} as ProductItem;
}
