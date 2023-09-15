import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-payment',
  templateUrl: './user-payment.component.html',
  styleUrls: ['./user-payment.component.css']
})
export class UserPaymentComponent implements OnInit{

  constructor(private router: Router){}

  ngOnInit(): void {
    setTimeout(() => {
      this.router.navigate(['/home']);
    }, 3000)
  }
}
