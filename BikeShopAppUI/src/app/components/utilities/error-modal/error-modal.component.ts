import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CustomError } from 'src/app/interfaces/customError';
import { ErrorService } from 'src/app/services/error.service';

@Component({
  selector: 'app-error-modal',
  templateUrl: './error-modal.component.html',
  styleUrls: ['./error-modal.component.css']
})
export class ErrorModalComponent implements OnInit{
  errorMessage: string = "";
  title: string = "";

  constructor(private errorService: ErrorService, private changeDetectionRef: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.errorService.GetCurrentError().subscribe((error: CustomError) => 
      {
        if(error.errors == undefined){
          this.errorMessage = error.detail;
        }
        else
        {
          for(var keys = Object.keys(error.errors), i = 0; i < keys.length; i++)
          {
            this.errorMessage += error.errors[keys[i]];

            if(i != keys.length - 1){
              this.errorMessage += " | ";
            }
          }
        }
        
        this.title = `${error.status}: ${error.title}`;
        this.changeDetectionRef.detectChanges();
      });
  }

  close()
  {
    this.errorMessage = "";
  }
}
