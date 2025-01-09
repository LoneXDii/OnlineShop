import {Component, EventEmitter, Output} from '@angular/core';

@Component({
  selector: 'app-product-form-category-selector',
  imports: [],
  templateUrl: './product-form-category-selector.component.html',
  styleUrl: './product-form-category-selector.component.css'
})
export class ProductFormCategorySelectorComponent {
  @Output() categorySelected = new EventEmitter<number>();

  categories = [
    { id: 1, name: 'Категория 1' },
    { id: 2, name: 'Категория 2' }
  ];

  onCategoryChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const categoryId = Number(selectElement.value);
    this.categorySelected.emit(categoryId);
  }
}
