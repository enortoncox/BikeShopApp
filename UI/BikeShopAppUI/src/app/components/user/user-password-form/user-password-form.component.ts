import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/interfaces/user';
import { UserPassword } from 'src/app/interfaces/userPassword';
import { AuthService } from 'src/app/services/auth.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-user-password-form',
  templateUrl: './user-password-form.component.html',
  styleUrls: ['./user-password-form.component.css']
})
export class UserPasswordFormComponent implements OnInit{
  passwordForm!: FormGroup;
  oldPassword: string = "";
  newPassword: string = "";
  currentUser: User;
  userPassword: UserPassword = {} as UserPassword;

  constructor(private usersService: UsersService, private router: Router, private route: ActivatedRoute, private authService: AuthService){
    this.currentUser = route.snapshot.data['currentUser'];
  }
  
  ngOnInit(): void {
    this.passwordForm = new FormGroup({
      oldPassword: new FormControl("", Validators.required),
      newPassword: new FormControl("", Validators.required)
    });
  }

  onSubmit()
  {
    this.oldPassword = this.passwordForm.value.oldPassword;
    this.newPassword = this.passwordForm.value.newPassword;

    this.userPassword = {userId: this.currentUser.userId, password: this.oldPassword};

    this.authService.ConfirmUserPassword(this.userPassword).subscribe((valid) => 
    {
      if(valid == false)
      {
        throw new Error("Old password wrong!");
      }

      if(this.oldPassword == this.newPassword)
      {
        throw new Error("New password can't be the same as old.");
      }

      this.userPassword = {password: this.newPassword, userId: this.currentUser.userId};

      this.usersService.UpdateUserPassword(this.userPassword).subscribe(() => 
        {
          this.router.navigate(['/user/account']);
        })
    });
  }
}
