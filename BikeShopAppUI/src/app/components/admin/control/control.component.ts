import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Product } from 'src/app/interfaces/product';
import { User } from 'src/app/interfaces/user';
import { AuthService } from 'src/app/services/auth.service';
import { ProductsService } from 'src/app/services/products.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-control',
  templateUrl: './control.component.html',
  styleUrls: ['./control.component.css']
})
export class ControlComponent implements OnInit{
  selectedUserId: string = "0";
  selectedProductId: string = "0";
  selectedUserIsAdmin: boolean = false;
  adminButtonText: string = "";
  users: User[] = [];
  products: Product[] = [];
  alphabet: string[] = ["a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"];
  currentProductLetter: string = "A";
  currentUserLetter: string = "A";

  constructor(private usersService: UsersService, private productsService: ProductsService, private authService: AuthService, private changeDetectionRef: ChangeDetectorRef) {}
  
  ngOnInit(): void {
    this.GetAllProductsThatStartWithLetter("A");
    this.GetAllUsersThatStartWithLetter("A");
  }

  GetAllProductsThatStartWithLetter(letter: string)
  {
    this.currentProductLetter = letter;

    this.productsService.GetAllProductsThatStartWithLetter(letter).subscribe(productsData => 
      {
        this.products = productsData;

        if(this.products.length > 0)
        {
          this.selectedProductId = this.products[0].productId;
        }
      });
  }

  GetAllUsersThatStartWithLetter(letter: string)
  {
    this.currentUserLetter = letter;

    this.usersService.GetAllUsersThatStartWithLetter(letter).subscribe(usersData => 
      {
        this.users = usersData;

        if(this.users.length > 0)
        {
          this.selectedUserId = this.users[0].userId;
          this.CheckIfAdmin(this.selectedUserId);
        }
      });
  }

  DeleteProduct(productId: string)
  {
    this.productsService.DeleteProduct(productId).subscribe(() => {
      this.GetAllProductsThatStartWithLetter(this.currentProductLetter);
    })
  }

  DeleteUser(userId: string)
  {
    this.usersService.DeleteUser(userId).subscribe(() => {
      this.GetAllUsersThatStartWithLetter(this.currentUserLetter);
    })
  }

  ChangeAdminStatus(userId: string)
  {
    this.authService.ChangeAdminStatus(userId).subscribe(() => {
      this.CheckIfAdmin(userId);
    })
  }

  CheckIfAdmin(userId: string)
  {
    this.authService.IsUserAdmin(userId).subscribe((res) => 
    {
      if(res)
      {
        this.selectedUserIsAdmin = true;
        this.adminButtonText = "Demote to User";
      }
      else
      {
        this.selectedUserIsAdmin = false;
        this.adminButtonText = "Promote to Admin";
      }
    })
  }

  UpdateView(){
    this.changeDetectionRef.detectChanges();
  }
}
