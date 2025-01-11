import {Component, inject, OnInit} from '@angular/core';
import {CartService} from '../../../../data/services/cart.service';

@Component({
  selector: 'app-nav-cart',
  imports: [],
  templateUrl: './nav-cart.component.html',
  styleUrl: './nav-cart.component.css'
})
export class NavCartComponent implements OnInit {
  cartService = inject(CartService);
  itemsCount: number = 0;
  totalCost: number = 0;

  ngOnInit() {
    this.cartService.cartItemsCount$
      .subscribe((count) => {this.itemsCount = count;});
    this.cartService.cartTotalCost$
      .subscribe((count) => {this.totalCost = count;});
  }
}
