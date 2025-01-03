import {Component, Input} from '@angular/core';
import {Category} from '../../../../data/interfaces/category.interface';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-category-card',
  imports: [
    RouterLink
  ],
  templateUrl: './category-card.component.html',
  styleUrl: './category-card.component.css'
})
export class CategoryCardComponent {
  @Input() category!: Category;
}
