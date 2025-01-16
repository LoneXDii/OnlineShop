import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import {Order} from '../../../../data/interfaces/cart/order.interface';
import {DatePipe} from '@angular/common';
import {RouterLink} from '@angular/router';
import {OrdersService} from '../../../../data/services/orders.service';

@Component({
  selector: 'app-admin-orders-list-item',
  imports: [
    DatePipe,
    RouterLink
  ],
  templateUrl: './admin-orders-list-item.component.html',
  styleUrl: './admin-orders-list-item.component.css'
})
export class AdminOrdersListItemComponent {
  @Input() order!: Order;
  @Output() orderChanged = new EventEmitter<void>();
  ordersService = inject(OrdersService);

  onConfirm(){
    this.ordersService.confirmOrder(this.order.id)
      .subscribe(() => this.orderChanged.emit());
  }

  onComplete(){
    this.ordersService.completeOrder(this.order.id)
      .subscribe(() => this.orderChanged.emit());
  }

  onCancel(){
    this.ordersService.cancelOrder(this.order.id)
      .subscribe(() => this.orderChanged.emit());
  }
}
