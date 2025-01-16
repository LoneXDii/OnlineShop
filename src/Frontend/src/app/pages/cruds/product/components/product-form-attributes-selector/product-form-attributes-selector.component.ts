import {Component, EventEmitter, inject, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {CategoriesService} from '../../../../../data/services/categories.service';
import {AttributeAllValues} from '../../../../../data/interfaces/catalog/attributeAllValues.interface';
import {
  ProductAttributeValueCreationModalComponent
} from '../product-attribute-value-creation-modal/product-attribute-value-creation-modal.component';
import {NgIf} from '@angular/common';
import {Category} from '../../../../../data/interfaces/catalog/category.interface';
import {AttributeValue} from '../../../../../data/interfaces/catalog/attributeValue.interface';

@Component({
  selector: 'app-product-form-attributes-selector',
  imports: [
    ProductAttributeValueCreationModalComponent,
    NgIf
  ],
  templateUrl: './product-form-attributes-selector.component.html',
  styleUrl: './product-form-attributes-selector.component.css'
})
export class ProductFormAttributesSelectorComponent implements OnInit, OnChanges {
  @Input() categoryId!: number;
  @Input() selectedAttributeValues?: AttributeValue[];
  @Output() attributesSelected = new EventEmitter<number[]>();
  categoriesService = inject(CategoriesService);
  attributeValues: AttributeAllValues[] = [];
  selectedAttributes: { [key: number]: string | null } = {};
  isModalVisible = false;
  attributeForValueAdding: Category | undefined = undefined;
  private isInitialized = false;

  ngOnInit() {
    this.updateAttributeValues();
    if (this.selectedAttributeValues) {
      this.initializeSelectedAttributes();
    }
    this.isInitialized = true;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['categoryId']) {
      if (this.isInitialized) {
        this.updateAttributeValues(true);
      } else {
        this.updateAttributeValues();
      }
    }
  }

  updateAttributeValues(reset = false) {
    this.categoriesService.getCategoryAttributesValues(this.categoryId)
      .subscribe(val => {
        this.attributeValues = val;

        if (reset || !this.selectedAttributeValues || this.selectedAttributeValues.length === 0) {
          this.resetSelectedAttributes();
        }

        this.updateAttributesArray();
      });
  }

  initializeSelectedAttributes() {
    this.selectedAttributes = {};
    for (let av of this.selectedAttributeValues!) {
      this.selectedAttributes[av.attributeId] = String(av.valueId);
    }
    this.updateAttributesArray();
  }

  resetSelectedAttributes() {
    this.selectedAttributes = {};
    for (let av of this.attributeValues) {
      this.selectedAttributes[av.attribute.id] = "0";
    }
  }

  onSelectAttribute(attributeId: number, valueId: string) {
    if (valueId === "0") {
      this.attributeForValueAdding = this.attributeValues
        .find(attr => attr.attribute.id === attributeId)?.attribute;
      this.openModal();
    }

    this.selectedAttributes[attributeId] = valueId;
    this.updateAttributesArray();
  }

  updateAttributesArray() {
    const keys = Object.keys(this.selectedAttributes);
    const values = Object.values(this.selectedAttributes)
      .filter((value): value is string => value !== null);

    const data = keys.concat(values);

    this.attributesSelected.emit(data.map(Number));
  }

  getSelectedValue(event: Event): string {
    const target = event.target as HTMLSelectElement;

    return target.value;
  }

  openModal() {
    this.isModalVisible = true;
  }

  closeModal() {
    this.isModalVisible = false;
    this.categoriesService.getCategoryAttributesValues(this.categoryId)
      .subscribe(val => this.attributeValues = val);
  }

  isSelectedValue(attributeId: number, valueId: number): boolean {
    const selectedAttribute = this.selectedAttributeValues?.find(attr => attr.attributeId === attributeId);

    return selectedAttribute ? selectedAttribute.valueId === valueId : false;
  }
}
