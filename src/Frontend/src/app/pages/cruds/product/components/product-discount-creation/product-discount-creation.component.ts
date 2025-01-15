import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import {AbstractControl, FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {ProductsService} from '../../../../../data/services/products.service';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-product-discount-creation',
  imports: [
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './product-discount-creation.component.html',
  styleUrl: './product-discount-creation.component.css'
})
export class ProductDiscountCreationComponent {
  @Input() productId!: number;
  @Output() onUpdate = new EventEmitter<number>();
  productsService = inject(ProductsService);

  form = new FormGroup({
    percent: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(100)]),
    endDate: new FormControl<string | null>(null, [Validators.required, this.dateValidator]),
  });

  dateValidator(control: FormControl): { [key: string]: boolean } | null {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const selectedDate = new Date(control.value);

    return selectedDate < today ? { 'invalidDate': true } : null;
  }

  onSubmit() {
    if(!this.form.valid) {
      return;
    }

    const {percent, endDate} = this.form.value;

    if(!percent || !endDate) {
      return;
    }

    this.productsService.addDiscount({
      productId: this.productId,
      percent: percent,
      endDate: new Date(endDate)}).subscribe(() => {
        this.onUpdate.emit(this.productId);
    });
  }
}
