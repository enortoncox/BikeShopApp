import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationResponse } from 'src/app/interfaces/authenticationResponse';
import { UserRegister } from 'src/app/interfaces/userRegister';
import { IDeactivateComponent } from 'src/app/routeGuards/form-leave.guard';
import { AuthService } from 'src/app/services/auth.service';
import { FilesService } from 'src/app/services/files.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, IDeactivateComponent{
  registerForm!: FormGroup;
  user: UserRegister = {} as UserRegister;
  fileName: string = "";
  dbPath: string = "";
  isSubmitting: boolean;
  domain: string = "";
  noImage: string = "";

  constructor(private authService: AuthService, private router: Router, private filesService: FilesService)
  {
    this.isSubmitting = false;
    this.domain = environment.domain;
    this.noImage = environment.noImage;
  }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      name: new FormControl("", Validators.required),
      address: new FormControl("", Validators.required),
      email: new FormControl("", [Validators.required, Validators.email]),
      password: new FormControl("", Validators.required),
      confirmPassword: new FormControl("", Validators.required),
    });
  }

  onSubmit()
  {
    this.user.name = this.registerForm.value.name;
    this.user.address = this.registerForm.value.address;
    this.user.email = this.registerForm.value.email;
    this.user.password = this.registerForm.value.password;
    this.user.confirmPassword = this.registerForm.value.confirmPassword;

    this.authService.Register(this.user).subscribe((response: AuthenticationResponse) => 
      {
        this.authService.SetCurrentUser(response.user, response.isAdmin);
        localStorage["token"] = response.token;
        localStorage["refreshToken"] = response.refreshToken;
        this.isSubmitting = true;
        this.router.navigate(['/home']);
      });
  }

  UploadImage(files: any)
  {
      if(files.length == 0)
      {
        return;
      }

      let fileToUpload = files[0];
      this.fileName = fileToUpload.name;

      let formData = new FormData();
      formData.append("file", fileToUpload, fileToUpload.name);

      this.filesService.UploadFile(formData).subscribe((res: any) => 
      {
        this.dbPath = res.dbPath;
        this.user.imgPath = this.dbPath;
      })
  }

  canExit()
  {
    if((!this.registerForm.pristine || this.fileName != "") && this.isSubmitting == false)
    {
      if(confirm("You have unsaved changes. Are you sure you want to leave?"))
      {
        if(this.dbPath != "")
        {
          this.filesService.DeleteFile(this.dbPath).subscribe(() => 
          {
            return true;
          });
        }
      }
      else
      {
        return false;
      }
    }
    return true;
  }
  
  noImageFound(e: any){
    e.target.src = this.noImage;
  }
}
