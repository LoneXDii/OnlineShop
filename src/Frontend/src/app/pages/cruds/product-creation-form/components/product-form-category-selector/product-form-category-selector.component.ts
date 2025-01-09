import {Component, EventEmitter, inject, OnInit, Output} from '@angular/core';
import {CategoriesService} from '../../../../../data/services/categories.service';
import {Category} from '../../../../../data/interfaces/catalog/category.interface';

@Component({
  selector: 'app-product-form-category-selector',
  imports: [],
  templateUrl: './product-form-category-selector.component.html',
  styleUrl: './product-form-category-selector.component.css'
})
export class ProductFormCategorySelectorComponent implements OnInit {
  @Output() categorySelected = new EventEmitter<number>();
  categoriesService = inject(CategoriesService);
  categories: Category[] = []

  ngOnInit(){
    this.categoriesService.getCategories()
      .subscribe(val => this.categories = val);
  }

  onCategoryChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const categoryId = Number(selectElement.value);
    this.categorySelected.emit(categoryId);
  }
}
