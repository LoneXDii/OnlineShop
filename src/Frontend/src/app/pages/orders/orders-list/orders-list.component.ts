import {Component, inject, OnInit} from '@angular/core';
import {OrdersService} from '../../../data/services/orders.service';
import {Order} from '../../../data/interfaces/cart/order.interface';
import {JsonPipe, NgIf} from '@angular/common';

@Component({
  selector: 'app-orders-list',
  imports: [
    NgIf,
    JsonPipe
  ],
  templateUrl: './orders-list.component.html',
  styleUrl: './orders-list.component.css'
})
export class OrdersListComponent implements OnInit {
  ordersService = inject(OrdersService);
  orders: Order[] | null = null;

  ngOnInit() {
    this.ordersService.getOrders()
      .subscribe(orders => this.orders = orders);
  }
}
