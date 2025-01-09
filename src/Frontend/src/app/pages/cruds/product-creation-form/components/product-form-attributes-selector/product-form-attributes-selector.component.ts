import {Component, EventEmitter, inject, Input, OnChanges, OnInit, Output} from '@angular/core';
import {CategoriesService} from '../../../../../data/services/categories.service';
import {AttributeAllValues} from '../../../../../data/interfaces/catalog/attributeAllValues.interface';

@Component({
  selector: 'app-product-form-attributes-selector',
  imports: [],
  templateUrl: './product-form-attributes-selector.component.html',
  styleUrl: './product-form-attributes-selector.component.css'
})
export class ProductFormAttributesSelectorComponent implements OnInit, OnChanges {
  @Input() categoryId!: number;
  @Output() attributesSelected = new EventEmitter<string[]>();
  categoriesService = inject(CategoriesService);
  attributeValues: AttributeAllValues[] = [];
  selectedAttributes: { [key: number]: string | null } = {};

  ngOnInit() {
    this.updateAttributeValues()
  }

  ngOnChanges() {
    this.updateAttributeValues();
  }

  updateAttributeValues() {
    this.categoriesService.getCategoryAttributesValues(this.categoryId)
      .subscribe(val => {
        this.attributeValues = val
      });
  }

  onSelectAttribute(categoryId: number, attributeId: string) {
    this.selectedAttributes[categoryId] = attributeId;
    this.updateAttributesArray();
  }

  updateAttributesArray() {
    const keys = Object.keys(this.selectedAttributes);
    const values = Object.values(this.selectedAttributes);
    //@ts-ignore
    const data = keys.concat(values);
    //@ts-ignore
    this.attributesSelected.emit(data);
  }

  getSelectedValue(event: Event): string {
    const target = event.target as HTMLSelectElement;
    return target.value;
  }
}
