import {Component, inject, Input, OnInit} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {CategoriesService} from '../../../../data/services/categories.service';
import {AttributeAllValues} from '../../../../data/interfaces/attributeAllValues.interface';
import {Category} from '../../../../data/interfaces/category.interface';
import {
  SidebarAttributeValuesSelectorComponent
} from '../sidebar-attribute-values-selector/sidebar-attribute-values-selector.component';

@Component({
  selector: 'app-sidebar',
  imports: [
    FormsModule,
    SidebarAttributeValuesSelectorComponent
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit{
  @Input() category!: Category;
  categoriesService = inject(CategoriesService);
  attributes: AttributeAllValues[] = [];
  minPrice: number | null = null;
  maxPrice: number | null = null;

  ngOnInit() {
    if (this.category) {
      this.categoriesService.getCategoryAttributesValues(this.category.id)
        .subscribe(val => this.attributes = val);
    }
  }
}
