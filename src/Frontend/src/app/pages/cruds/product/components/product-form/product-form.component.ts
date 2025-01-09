import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormGroup, ReactiveFormsModule} from '@angular/forms';
import {
  ProductFormAttributesSelectorComponent
} from '../product-form-attributes-selector/product-form-attributes-selector.component';
import {NgIf} from '@angular/common';
import {
  ProductFormCategorySelectorComponent
} from '../product-form-category-selector/product-form-category-selector.component';
import {AttributeValue} from '../../../../../data/interfaces/catalog/attributeValue.interface';

@Component({
  selector: 'app-product-form',
  imports: [
    ProductFormAttributesSelectorComponent,
    NgIf,
    ProductFormCategorySelectorComponent,
    ReactiveFormsModule
  ],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css'
})
export class ProductFormComponent implements OnInit {
  @Input() selectedCategoryId?: number;
  @Input() form!: FormGroup;
  @Input() selectedAttributeValues?: AttributeValue[];
  @Output() submitForm = new EventEmitter<FormData>();

  categoryId: number | null = null;

  ngOnInit(){
    if(this.selectedCategoryId) {
      this.categoryId = this.selectedCategoryId;
    }
  }

  onCategorySelected(newCategoryId: number) {
    this.categoryId = newCategoryId;
    this.selectedCategoryId = undefined;
  }

  onAttributesSelected(selectedAttributes: number[]) {
    if (this.categoryId) {
      selectedAttributes.push(this.categoryId);
      this.form.patchValue({
        attributes: selectedAttributes,
      });
    }
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    this.form.patchValue({
      image: file,
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const formData = new FormData();

      formData.append('name', this.form.get('name')?.value || '');
      formData.append('price', this.form.get('price')?.value?.toString() || '');
      formData.append('quantity', this.form.get('quantity')?.value?.toString() || '');
      formData.append('image', this.form.get('image')?.value || '');

      const attributes = this.form.get('attributes')?.value || [];
      attributes.forEach((attr: number) => {
        formData.append('attributes[]', attr.toString());
      });

      this.submitForm.emit(formData);
    }
  }
}
