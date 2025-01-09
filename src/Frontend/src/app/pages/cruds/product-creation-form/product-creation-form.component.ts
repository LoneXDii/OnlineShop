import { Component } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators
} from '@angular/forms';
import {
  ProductFormAttributesSelectorComponent
} from './components/product-form-attributes-selector/product-form-attributes-selector.component';
import {NgIf} from '@angular/common';
import {
  ProductFormCategorySelectorComponent
} from './components/product-form-category-selector/product-form-category-selector.component';

@Component({
  selector: 'app-product-creation-form',
  imports: [
    ProductFormAttributesSelectorComponent,
    NgIf,
    ReactiveFormsModule,
    ProductFormCategorySelectorComponent
  ],
  templateUrl: './product-creation-form.component.html',
  styleUrl: './product-creation-form.component.css'
})
export class ProductCreationFormComponent {
  categoryId: number | null = null;

  form = new FormGroup({
    name: new FormControl<string | null>(null, Validators.required),
    price: new FormControl<number | null>(null, [Validators.required, Validators.min(1)]),
    quantity: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    image: new FormControl<File | null>(null),
    attributes: new FormControl<number[]>([], this.attributesValidator)
  });

  attributesValidator(control: AbstractControl): ValidationErrors | null {
    const attributes: number[] = control.value;

    if (attributes.length > 0 && attributes.every(attr => attr !== 0)) {
      return null;
    }

    return { invalidAttributes: true };
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    this.form.patchValue({
      image: file
    });
  }

  onCategorySelected(newCategoryId: number) {
    this.categoryId = newCategoryId;
  }

  onAttributesSelected(selectedAttributes: number[]) {
    if(this.categoryId) {
      selectedAttributes.push(this.categoryId);
      this.form.patchValue({
        attributes: selectedAttributes
      });
    }
  }

  onSubmit() {
    console.log('Form:', this.form.value);
  }
}
