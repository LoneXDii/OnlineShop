import {Component, Input} from '@angular/core';
import {CartProduct} from '../../../../../data/interfaces/cart/cartProduct.interface';

@Component({
  selector: 'app-order-product',
  imports: [],
  templateUrl: './order-product.component.html',
  styleUrl: './order-product.component.css'
})
export class OrderProductComponent {
  @Input() product!: CartProduct;
}
