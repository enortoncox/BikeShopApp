import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute, Params, RouterModule } from '@angular/router';
import { Category } from 'src/app/interfaces/category';

export interface CategoryLink
{
  name: string,
  params: Params
}

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: true,
    imports: [
      CommonModule,
      RouterModule
  ]
})
export class HomeComponent implements OnInit{

  categories: Category[] = [];
  categoriesLinks: CategoryLink[] = []; 

  constructor(private route: ActivatedRoute)
  {
    this.categories = this.route.snapshot.data['categoriesData'];
  }

  ngOnInit(): void {
    this.categories.forEach(category => this.categoriesLinks.push({name: category.name, params: {"category": category.categoryId}}));
  }
}
