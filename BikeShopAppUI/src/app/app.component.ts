import { ChangeDetectorRef, Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { ErrorModalComponent } from './components/utilities/error-modal/error-modal.component';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: true,
    imports: [
      RouterOutlet,
      HeaderComponent,
      ErrorModalComponent
  ]
})
export class AppComponent {
  title = 'BikeShopAppUI';

  constructor(private changeDetectionRef: ChangeDetectorRef){}

  UpdateView(){
    this.changeDetectionRef.detectChanges();
  }
}
