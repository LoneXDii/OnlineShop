import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import {AttributeAllValues} from '../../../../../data/interfaces/catalog/attributeAllValues.interface';
import {
  ProductAttributeValueCreationModalComponent
} from '../../../product/components/product-attribute-value-creation-modal/product-attribute-value-creation-modal.component';
import {NgIf} from '@angular/common';
import {CategoriesService} from '../../../../../data/services/categories.service';

@Component({
  selector: 'app-category-attribute-values',
  imports: [
    ProductAttributeValueCreationModalComponent,
    NgIf
  ],
  templateUrl: './category-attribute-values.component.html',
  styleUrl: './category-attribute-values.component.css'
})
export class CategoryAttributeValuesComponent {
  @Input() attributeValues!: AttributeAllValues;
  @Output() categoryUpdated = new EventEmitter<void>();
  categoriesService = inject(CategoriesService);
  isModalVisible = false;

  onDelete(valueId: number) {
    this.categoriesService.deleteCategory(valueId)
      .subscribe({
        next: () => this.categoryUpdated.emit(),
        error: err => alert("Can't delete value that is in use by any product")
      });
  }

  openModal() {
    this.isModalVisible = true;
  }

  closeModal() {
    this.isModalVisible = false;
    this.categoryUpdated.emit();
  }
}
