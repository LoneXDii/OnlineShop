import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import {Category} from '../../../../data/interfaces/catalog/category.interface';
import {CategoriesService} from '../../../../data/services/categories.service';

@Component({
  selector: 'app-category-list-item',
  imports: [],
  templateUrl: './category-list-item.component.html',
  styleUrl: './category-list-item.component.css'
})
export class CategoryListItemComponent {
  @Input() category!: Category;
  @Output() categoryDeleted = new EventEmitter<void>();
  categoriesService = inject(CategoriesService);

  onDelete(){
    this.categoriesService.deleteCategory(this.category.id)
      .subscribe(() => {
        alert('Category deleted successfully.');
        this.categoryDeleted.emit();
      });
  }
}
