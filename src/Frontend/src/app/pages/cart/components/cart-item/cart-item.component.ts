import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import {CartProduct} from '../../../../data/interfaces/cart/cartProduct.interface';
import {CartService} from '../../../../data/services/cart.service';

@Component({
  selector: '[app-cart-item]',
  imports: [],
  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.css'
})
export class CartItemComponent {
  @Input() product!: CartProduct;
  @Output() productChanged = new EventEmitter<void>();
  cartService = inject(CartService);

  onIncrease(){
    this.cartService.increaseProductQuantity(this.product.id, this.product.quantity + 1)
      .subscribe(() => this.productChanged.emit());
  }

  onDecrease(){
    this.cartService.decreaseProductQuantity(this.product.id)
      .subscribe(() => this.productChanged.emit());
  }

  onDelete(){
    this.cartService.removeProductFromCart(this.product.id)
      .subscribe(() => this.productChanged.emit());
  }
}
