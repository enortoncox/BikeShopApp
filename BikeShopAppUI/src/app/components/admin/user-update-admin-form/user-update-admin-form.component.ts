import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/interfaces/user';
import { IDeactivateComponent } from 'src/app/routeGuards/form-leave.guard';
import { FilesService } from 'src/app/services/files.service';
import { UsersService } from 'src/app/services/users.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-user-update-admin-form',
  templateUrl: './user-update-admin-form.component.html',
  styleUrls: ['./user-update-admin-form.component.css']
})
export class UserUpdateAdminFormComponent implements OnInit, IDeactivateComponent{
  userUpdateAdminForm!: FormGroup;
  user: User;
  fileName: string = "";
  dbPath: string = "";
  isSubmitting: boolean;
  orginalFilePath: string = "";
  newFilePath: string = "";
  domain: string = "";
  noImage: string = "";

  constructor(private usersService: UsersService, private router: Router, private route: ActivatedRoute, private filesService: FilesService){
    this.user = this.route.snapshot.data['userData'];
    this.dbPath = this.user.imgPath;
    this.fileName = this.dbPath.split("\\")[2];
    this.isSubmitting = false;
    this.domain = environment.domain;
    this.noImage = environment.noImage;
  }
  
  ngOnInit(): void {
    this.userUpdateAdminForm = new FormGroup({
      name: new FormControl(this.user.name, Validators.required),
      address: new FormControl(this.user.address, Validators.required),
      email: new FormControl(this.user.email, [Validators.required, Validators.email]),
    });

    this.orginalFilePath = this.user.imgPath;
  }

  onSubmit()
  {
    this.user.name = this.userUpdateAdminForm.value.name;
    this.user.address = this.userUpdateAdminForm.value.address;
    this.user.email = this.userUpdateAdminForm.value.email;

    this.usersService.UpdateUser(this.user).subscribe(() => 
      {
        if(this.newFilePath != "" && this.orginalFilePath.toLowerCase() != "resources\\images\\user.jpg")
        {
          this.filesService.DeleteFile(this.orginalFilePath).subscribe(() => {
            this.isSubmitting = true;
            this.router.navigate(['/admin/control']);
          });
        }
        else{
          this.isSubmitting = true;
          this.router.navigate(['/admin/control']);
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
        this.filesService.DeleteFile(this.newFilePath).subscribe(() => {});
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
        this.userUpdateAdminForm.markAsDirty();
      })
  }

  canExit()
  {
    if((!this.userUpdateAdminForm.pristine) && this.isSubmitting == false)
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
