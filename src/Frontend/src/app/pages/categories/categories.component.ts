import {Component, inject} from '@angular/core';
import {CategoriesService} from '../../data/services/categories.service';
import {Category} from '../../data/interfaces/category.interface';
import {CategoryCardComponent} from './componets/category-card/category-card.component';

@Component({
  selector: 'app-categories',
  imports: [
    CategoryCardComponent
  ],
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.css'
})
export class CategoriesComponent {
  categoriesService = inject(CategoriesService);

  categories: Category[] = [];

  constructor() {
    this.categoriesService.getCategories()
      .subscribe(val => this.categories = val);
  }
}
