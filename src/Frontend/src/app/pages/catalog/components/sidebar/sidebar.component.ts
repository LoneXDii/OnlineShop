import {Component, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {CategoriesService} from '../../../../data/services/categories.service';
import {AttributeAllValues} from '../../../../data/interfaces/catalog/attributeAllValues.interface';
import {Category} from '../../../../data/interfaces/catalog/category.interface';
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
  @Input() categoryId!: number;
  @Output() fetchProducts = new EventEmitter<{ minPrice?: number; maxPrice?: number; valuesIds?: number[] }>();

  category!: Category;
  categoriesService = inject(CategoriesService);
  attributes: AttributeAllValues[] = [];
  minPrice: number | undefined = undefined;
  maxPrice: number | undefined = undefined;
  selectedValues: { [key: number]: number } = {};

  ngOnInit() {
    this.categoriesService.getCategoryById(this.categoryId)
      .subscribe(val => this.category = val);

    this.categoriesService.getCategoryAttributesValues(this.categoryId)
      .subscribe(val => this.attributes = val);
  }

  onValueSelected(attributeId: number, value: number) {
    if (value === undefined) {
      delete this.selectedValues[attributeId];
    }
    else {
      this.selectedValues[attributeId] = value;
    }
  }

  onFindClick(){
    this.fetchProducts.emit({
      minPrice: this.minPrice,
      maxPrice: this.maxPrice,
      valuesIds: Object.values(this.selectedValues)
    });
  }
}
