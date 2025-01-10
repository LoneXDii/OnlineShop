import {Component, EventEmitter, Input, Output} from '@angular/core';
import {AttributeAllValues} from '../../../../../data/interfaces/catalog/attributeAllValues.interface';
import {
  ProductAttributeValueCreationModalComponent
} from '../../../product/components/product-attribute-value-creation-modal/product-attribute-value-creation-modal.component';
import {NgIf} from '@angular/common';

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
  isModalVisible = false;

  openModal() {
    this.isModalVisible = true;
  }

  closeModal() {
    this.isModalVisible = false;
    this.categoryUpdated.emit();
  }
}
