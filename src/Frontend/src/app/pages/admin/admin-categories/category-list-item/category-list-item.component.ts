import {Component, Input} from '@angular/core';
import {Category} from '../../../../data/interfaces/catalog/category.interface';

@Component({
  selector: 'app-category-list-item',
  imports: [],
  templateUrl: './category-list-item.component.html',
  styleUrl: './category-list-item.component.css'
})
export class CategoryListItemComponent {
  @Input() category!: Category;
}
