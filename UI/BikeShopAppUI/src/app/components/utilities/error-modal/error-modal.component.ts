import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ErrorService } from 'src/app/services/error.service';

@Component({
  selector: 'app-error-modal',
  templateUrl: './error-modal.component.html',
  styleUrls: ['./error-modal.component.css']
})
export class ErrorModalComponent implements OnInit{
  errorMessage: string = "";

  constructor(private errorService: ErrorService, private changeDetectionRef: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.errorService.GetCurrentError().subscribe(message => 
      {
        this.errorMessage = message;
        this.changeDetectionRef.detectChanges();
      });
  }

  close()
  {
    this.errorMessage = "";
  }
}
