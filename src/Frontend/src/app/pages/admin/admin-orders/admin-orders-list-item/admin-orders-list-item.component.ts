import {Component, Input} from '@angular/core';
import {Order} from '../../../../data/interfaces/cart/order.interface';
import {DatePipe} from '@angular/common';
import {RouterLink} from '@angular/router';

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
}
