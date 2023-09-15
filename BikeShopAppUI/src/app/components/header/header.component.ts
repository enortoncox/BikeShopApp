import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Params, Router } from '@angular/router';
import { Category } from 'src/app/interfaces/category';
import { User } from 'src/app/interfaces/user';
import { AuthService } from 'src/app/services/auth.service';
import { CategoriesService } from 'src/app/services/categories.service';

export interface NavItem
{
  name: string,
  link: string,
  exact: boolean,
  queryParams?: Params
}

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit{
  navItemList: NavItem[] = [];
  loggedInItemList: NavItem[] = [];
  loggedOutItemList: NavItem[] = [];
  categories: Category[] = [];
  currentUser: User = {} as User;
  isMenuOpen: boolean = false;
  buttonText: string = "x";

  constructor(private categoriesService: CategoriesService, private authService: AuthService, private router: Router){
    this.navItemList = [
      {name: "Home", link: "/home", exact: true}
    ]

    this.loggedInItemList = [
      {name: "Orders", link: "/user/orders", exact: true},
      {name: "Cart", link: "/user/cart", exact: true},
      {name: "Account", link: "user/account", exact: true},
    ]

    this.loggedOutItemList = [
      {name: "Login", link: "auth/login", exact: true},
      {name: "Register", link: "auth/register", exact: true},
    ]
  }

  ngOnInit(): void {
    this.categoriesService.GetCategories().subscribe(categoriesData => 
      {
        this.categories = categoriesData;
        this.categories.forEach(category => this.navItemList.push({name: category.name, link: `/products`, exact: true, queryParams: {"category": category.categoryId}}))
      });

      this.authService.GetCurrentUser().subscribe(user => {
        this.currentUser = user;

        if(this.authService.isLoggedInAdmin)
        {
          this.loggedInItemList = [
            {name: "Admin Panel", link: "/admin/control", exact: true},
            {name: "Orders", link: "/user/orders", exact: true},
            {name: "Cart", link: "/user/cart", exact: true},
            {name: this.currentUser.email, link: "user/account", exact: true},
          ]
        }
        else
        {
          this.loggedInItemList = [
            {name: "Orders", link: "/user/orders", exact: true},
            {name: "Cart", link: "/user/cart", exact: true},
            {name: this.currentUser.email, link: "user/account", exact: true},
          ]
        }
      });

      this.router.events.subscribe((event: any) => {
        if (event instanceof NavigationEnd){
           this.closeMenu(); 
         }
        })
  }

  logout()
  {
    this.authService.Logout();
  }

  toggleMenu()
  {
    this.isMenuOpen = !this.isMenuOpen;

    if(this.buttonText == 'x')
    {
      this.buttonText = '+'
    }
    else{
      this.buttonText = 'x'
    }
  }

  closeMenu()
  {
    this.isMenuOpen = false;
    this.buttonText = '+'
  }
}
