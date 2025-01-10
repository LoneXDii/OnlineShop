import {Component, inject, OnInit} from '@angular/core';
import {CategoriesService} from '../../../data/services/categories.service';
import {Category} from '../../../data/interfaces/catalog/category.interface';
import {CategoryListItemComponent} from './category-list-item/category-list-item.component';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-admin-categories',
  imports: [
    CategoryListItemComponent,
    RouterLink
  ],
  templateUrl: './admin-categories.component.html',
  styleUrl: './admin-categories.component.css'
})
export class AdminCategoriesComponent implements OnInit {
  categoryService = inject(CategoriesService);
  categories: Category[] = [];

  ngOnInit() {
    this.refreshCategories();
  }

  refreshCategories() {
    this.categoryService.getCategories()
      .subscribe(val => this.categories = val);
  }
}
