import {Component, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {CategoriesService} from '../../../../../data/services/categories.service';
import {AttributeAllValues} from '../../../../../data/interfaces/catalog/attributeAllValues.interface';
import {CategoryAttributeValuesComponent} from '../category-attribute-values/category-attribute-values.component';
import {NgIf} from '@angular/common';
import {
  ProductAttributeValueCreationModalComponent
} from '../../../product/components/product-attribute-value-creation-modal/product-attribute-value-creation-modal.component';
import {Category} from '../../../../../data/interfaces/catalog/category.interface';

@Component({
  selector: 'app-category-attributes-values',
  imports: [
    CategoryAttributeValuesComponent,
    NgIf,
    ProductAttributeValueCreationModalComponent
  ],
  templateUrl: './category-attributes-values.component.html',
  styleUrl: './category-attributes-values.component.css'
})
export class CategoryAttributesValuesComponent implements OnInit {
  @Input() category!: Category;
  @Output() categoryUpdated = new EventEmitter<void>();
  categoriesService = inject(CategoriesService);
  attributesValues: AttributeAllValues[] | null = null;
  isModalVisible = false;

  ngOnInit() {
    this.updateAttributesValues();
  }

  updateAttributesValues(){
    this.categoriesService.getCategoryAttributesValues(this.category.id)
      .subscribe(attributesValues => this.attributesValues = attributesValues);
  }

  openModal() {
    this.isModalVisible = true;
  }

  closeModal() {
    this.isModalVisible = false;
    this.updateAttributesValues();
    this.categoryUpdated.emit();
  }
}
