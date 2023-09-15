import { ChangeDetectorRef, Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'BikeShopAppUI';

  constructor(private changeDetectionRef: ChangeDetectorRef){}

  UpdateView(){
    this.changeDetectionRef.detectChanges();
  }
}
