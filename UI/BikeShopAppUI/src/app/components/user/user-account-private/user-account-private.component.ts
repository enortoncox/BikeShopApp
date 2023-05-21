import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/interfaces/user';

@Component({
  selector: 'app-user-account-private',
  templateUrl: './user-account-private.component.html',
  styleUrls: ['./user-account-private.component.css']
})
export class UserAccountPrivateComponent {

  currentUser: User;

  constructor(private route: ActivatedRoute) {
    this.currentUser = this.route.snapshot.data['currentUser'];
  }
}
