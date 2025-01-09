import {Component, inject, OnInit} from '@angular/core';
import {AbstractControl, FormControl, FormGroup, ValidationErrors, Validators} from '@angular/forms';
import {ProductsService} from '../../../../data/services/products.service';
import {ActivatedRoute, Router} from '@angular/router';
import {Product} from '../../../../data/interfaces/catalog/product.interface';
import {ProductFormComponent} from '../components/product-form/product-form.component';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-product-editing',
  imports: [
    ProductFormComponent,
    NgIf
  ],
  templateUrl: './product-editing.component.html',
  styleUrl: './product-editing.component.css'
})
export class ProductEditingComponent implements OnInit {
  productsService = inject(ProductsService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  productId: number | null = null;
  product: Product | null = null;

  form = new FormGroup({
    name: new FormControl<string | null>(null, Validators.required),
    price: new FormControl<number | null>(null, [Validators.required, Validators.min(1)]),
    quantity: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    image: new FormControl<File | null>(null),
    attributes: new FormControl<number[]>([], this.attributesValidator),
  });

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.productId = +params['id'];
      this.productsService.getProductById(this.productId)
        .subscribe(product => {
          this.product = product;
          this.form.patchValue({
            name: product.name,
            price: product.price,
            quantity: product.quantity
          })
        })
    });
  }

  attributesValidator(control: AbstractControl): ValidationErrors | null {
    const attributes: number[] = control.value;

    if (attributes.length > 0 && attributes.every(attr => attr !== 0)) {
      return null;
    }

    return { invalidAttributes: true };
  }

  onSubmit(formData: FormData) {
    // this.productsService.createProduct(formData)
    //   .subscribe(() => this.router.navigate(['/admin']));
  }
}
