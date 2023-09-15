import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/interfaces/user';
import { IDeactivateComponent } from 'src/app/routeGuards/form-leave.guard';
import { FilesService } from 'src/app/services/files.service';
import { UsersService } from 'src/app/services/users.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-user-update-form',
  templateUrl: './user-update-form.component.html',
  styleUrls: ['./user-update-form.component.css']
})
export class UserUpdateFormComponent implements OnInit, IDeactivateComponent{
  userUpdateForm!: FormGroup;
  user: User;
  fileName: string = "";
  dbPath: string = "";
  isSubmitting: boolean;
  orginalFilePath: string = "";
  newFilePath: string = "";
  domain: string = "";
  noImage: string = "";

  constructor(private usersService: UsersService, private router: Router, private route: ActivatedRoute, private filesService: FilesService){
    this.domain = environment.domain;
    this.noImage = environment.noImage;
    this.user = this.route.snapshot.data['currentUser'];
    this.dbPath = this.user.imgPath;
    this.fileName = this.dbPath.split("\\")[2];
    this.isSubmitting = false;
  }

  ngOnInit(): void {
    this.userUpdateForm = new FormGroup({
      name: new FormControl(this.user.name, Validators.required),
      address: new FormControl(this.user.address, Validators.required),
      email: new FormControl(this.user.email, Validators.required),
    });

    this.orginalFilePath = this.user.imgPath;
  }

  onSubmit()
  {
    this.user.name = this.userUpdateForm.value.name;
    this.user.address = this.userUpdateForm.value.address;
    this.user.email = this.userUpdateForm.value.email;

    this.usersService.UpdateUser(this.user).subscribe(() => 
      {
        if(this.newFilePath != "" && this.orginalFilePath.toLowerCase() != "resources\\images\\user.jpg")
        {
          this.filesService.DeleteFile(this.orginalFilePath).subscribe(() => {
            this.isSubmitting = true;
            this.router.navigate(['/user/account']);
          });
        }
        else{
          this.isSubmitting = true;
          this.router.navigate(['/user/account']);
        }

      });
  }

  UploadImage(files: any)
  {
      if(files.length == 0)
      {
        return;
      }

      if(this.newFilePath != "")
      {
        this.filesService.DeleteFile(this.newFilePath).subscribe();
      }

      let fileToUpload = files[0];
      this.fileName = fileToUpload.name;

      let formData = new FormData();
      formData.append("file", fileToUpload, fileToUpload.name);

      this.filesService.UploadFile(formData).subscribe((res: any) => 
      {
        this.dbPath = res.dbPath;
        this.user.imgPath = this.dbPath;
        this.newFilePath = this.dbPath;
        this.userUpdateForm.markAsDirty();
      })
  }

  canExit()
  {
    if((!this.userUpdateForm.pristine) && this.isSubmitting == false)
    {
       if(confirm("You have unsaved changes. Are you sure you want to leave?"))
       {
          if(this.newFilePath != "")
          {
            this.filesService.DeleteFile(this.newFilePath).subscribe(() => 
            {
              this.user.imgPath = this.orginalFilePath;
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
