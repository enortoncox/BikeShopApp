import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/interfaces/user';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-user-account-private',
  templateUrl: './user-account-private.component.html',
  styleUrls: ['./user-account-private.component.css']
})
export class UserAccountPrivateComponent {

  currentUser: User;
  domain: string = "";
  noImage: string  = "";

  constructor(private route: ActivatedRoute) {
    this.currentUser = this.route.snapshot.data['currentUser'];
    this.domain = environment.domain;
    this.noImage = environment.noImage;
  }

  noImageFound(e: any){
    e.target.src = this.noImage;
  }
}
