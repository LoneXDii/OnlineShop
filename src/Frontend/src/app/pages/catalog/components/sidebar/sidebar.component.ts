import {Component, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
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
  @Output() fetchProducts = new EventEmitter<{ minPrice?: number; maxPrice?: number; ValuesIds?: number[] }>();
  categoriesService = inject(CategoriesService);
  attributes: AttributeAllValues[] = [];
  minPrice: number | undefined = undefined;
  maxPrice: number | undefined = undefined;
  selectedValues: { [key: number]: number } = {};

  ngOnInit() {
    if (this.category) {
      this.categoriesService.getCategoryAttributesValues(this.category.id)
        .subscribe(val => this.attributes = val);
    }
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
      ValuesIds: Object.values(this.selectedValues)
    });
    console.log("clicked")
  }
}
