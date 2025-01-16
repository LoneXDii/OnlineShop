import {Component, inject, Input} from '@angular/core';
import {Order} from '../../../../../data/interfaces/cart/order.interface';
import {DatePipe} from '@angular/common';
import {RouterLink} from '@angular/router';
import {AuthService} from '../../../../../data/services/auth.service';

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
  authService = inject(AuthService);
}
