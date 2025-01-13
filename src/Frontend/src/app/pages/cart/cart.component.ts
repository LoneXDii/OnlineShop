import {Component, inject, OnInit} from '@angular/core';
import {CartService} from '../../data/services/cart.service';
import {Cart} from '../../data/interfaces/cart/cart.interface';
import {NgIf} from '@angular/common';
import {CartItemComponent} from './components/cart-item/cart-item.component';
import {OrdersService} from '../../data/services/orders.service';

@Component({
  selector: 'app-cart',
  imports: [
    NgIf,
    CartItemComponent
  ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  cartService = inject(CartService);
  ordersService = inject(OrdersService);
  cart: Cart | null = null;

  ngOnInit() {
    this.refreshCart();
  }

  refreshCart(){
    this.cartService.getCart()
      .subscribe(cart => this.cart = cart);
  }

  clearCart(){
    this.cartService.clearCart();
    this.refreshCart();
  }

  createOrder(){
    this.ordersService.createOrder()
      .subscribe(() => this.refreshCart());
  }
}
