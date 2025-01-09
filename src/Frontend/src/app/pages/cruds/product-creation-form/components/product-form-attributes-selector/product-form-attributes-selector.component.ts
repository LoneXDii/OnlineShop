import {Component, EventEmitter, inject, Input, OnChanges, OnInit, Output} from '@angular/core';
import {CategoriesService} from '../../../../../data/services/categories.service';
import {AttributeAllValues} from '../../../../../data/interfaces/catalog/attributeAllValues.interface';
import {
  ProductAttributeValueCreationModalComponent
} from '../product-attribute-value-creation-modal/product-attribute-value-creation-modal.component';
import {NgIf} from '@angular/common';
import {Category} from '../../../../../data/interfaces/catalog/category.interface';

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
  @Output() attributesSelected = new EventEmitter<number[]>();
  categoriesService = inject(CategoriesService);
  attributeValues: AttributeAllValues[] = [];
  selectedAttributes: { [key: number]: string | null } = {};
  isModalVisible = false;
  attributeForValueAdding: Category | undefined = undefined;

  ngOnInit() {
    this.updateAttributeValues()
  }

  ngOnChanges() {
    this.updateAttributeValues();
  }

  updateAttributeValues() {
    this.categoriesService.getCategoryAttributesValues(this.categoryId)
      .subscribe(val => {
        this.attributeValues = val;
        this.selectedAttributes = {};
        for(let av of this.attributeValues) {
          this.selectedAttributes[av.attribute.id] = "0";
        }
        this.updateAttributesArray();
      });
  }

  onSelectAttribute(attributeId: number, valueId: string) {
    if(valueId === "0"){
      this.attributeForValueAdding = this.attributeValues
        .find(attr => attr.attribute.id === attributeId)?.attribute;
      this.openModal();

      return;
    }

    this.selectedAttributes[attributeId] = valueId;
    this.updateAttributesArray();
  }

  updateAttributesArray() {
    const keys = Object.keys(this.selectedAttributes);
    const values = Object.values(this.selectedAttributes);
    //@ts-ignore
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
      .subscribe(val => this.attributeValues = val)
  }
}
