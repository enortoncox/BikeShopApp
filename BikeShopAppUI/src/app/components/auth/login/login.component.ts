import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationResponse } from 'src/app/interfaces/authenticationResponse';
import { UserLogin } from 'src/app/interfaces/userLogin';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  loginForm!: FormGroup;
  user: UserLogin = {} as UserLogin;

  constructor(private authService: AuthService, private router: Router){}

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      email: new FormControl("", Validators.required),
      password: new FormControl("", Validators.required)
    });
  }

  onSubmit()
  {
    this.user.email = this.loginForm.value.email;
    this.user.password = this.loginForm.value.password;

    this.authService.Login(this.user).subscribe((response: AuthenticationResponse) => 
      {
        this.authService.SetCurrentUser(response.user, response.isAdmin);
        localStorage["token"] = response.token;
        localStorage["refreshToken"] = response.refreshToken;
        this.router.navigate(['/home']);
      });
  }
}
