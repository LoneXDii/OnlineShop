import {Component, Input} from '@angular/core';
import {Order} from '../../../../../data/interfaces/cart/order.interface';
import {DatePipe} from '@angular/common';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-orders-list-item',
  imports: [
    DatePipe,
    RouterLink,
  ],
  templateUrl: './orders-list-item.component.html',
  styleUrl: './orders-list-item.component.css'
})
export class OrdersListItemComponent {
  @Input() order!: Order;
}
