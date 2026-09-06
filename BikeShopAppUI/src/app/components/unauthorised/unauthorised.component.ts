import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
    selector: 'app-unauthorised',
    templateUrl: './unauthorised.component.html',
    styleUrls: ['./unauthorised.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: true,
    imports: [RouterModule]
})
export class UnauthorisedComponent {

}
