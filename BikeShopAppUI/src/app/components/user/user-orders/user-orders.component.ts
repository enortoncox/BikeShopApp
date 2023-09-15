import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/interfaces/order';
import { OrdersProducts } from 'src/app/interfaces/ordersProducts';
import { User } from 'src/app/interfaces/user';
import { OrdersServices } from 'src/app/services/orders.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-user-orders',
  templateUrl: './user-orders.component.html',
  styleUrls: ['./user-orders.component.css']
})
export class UserOrdersComponent implements OnInit{
  currentUser: User;
  orders: Order[] = [];
  ordersProducts: OrdersProducts[] = [];
  currentPage: number = 1;
  pageResults: number = 5;
  pageNumbers: Number[] = [];
  domain: string = "";
  noImage: string = "";

  constructor(private ordersService: OrdersServices, private route: ActivatedRoute) {
    this.domain = environment.domain;
    this.noImage = environment.noImage;
    this.currentUser = this.route.snapshot.data['currentUser'];
  }

  ngOnInit(): void {
    this.ordersService.GetOrdersFromUserByPage(this.currentUser.userId, this.currentPage, this.pageResults).subscribe(res => 
      {
        this.orders = res.orders;
        this.pageNumbers = [...Array(res.pages).keys()].map(el => el + 1);
        this.currentPage = 1;

        console.log(this.pageNumbers);

        this.orders.forEach(order => {
          this.ordersService.GetAllProductsInOrder(order.orderId).subscribe(productsData => 
            {
              this.ordersProducts.push({orderId: order.orderId, products: productsData});
            });
          })
      });
  }

  changePage(page: any)
  {
      this.currentPage = page;

      this.ordersService.GetOrdersFromUserByPage(this.currentUser.userId, this.currentPage, this.pageResults).subscribe(res => 
        {
          this.orders = res.orders;

          this.orders.forEach(order => {
            this.ordersService.GetAllProductsInOrder(order.orderId).subscribe(productsData => 
              {
                this.ordersProducts.push({orderId: order.orderId, products: productsData});
              });
            })
        });
  }

  noImageFound(e: any){
    e.target.src = this.noImage;
  }
}
