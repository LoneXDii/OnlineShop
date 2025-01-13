import {Component, inject, OnInit} from '@angular/core';
import {OrdersService} from '../../../data/services/orders.service';
import {Order} from '../../../data/interfaces/cart/order.interface';
import {NgIf} from '@angular/common';
import {PaginatedOrder} from '../../../data/interfaces/cart/paginetedOrder.interface';
import {OrdersListItemComponent} from './components/orders-list-item/orders-list-item.component';
import {PaginationComponent} from '../../common/pagination/pagination.component';

@Component({
  selector: 'app-orders-list',
  imports: [
    NgIf,
    OrdersListItemComponent,
    PaginationComponent
  ],
  templateUrl: './orders-list.component.html',
  styleUrl: './orders-list.component.css'
})
export class OrdersListComponent implements OnInit {
  ordersService = inject(OrdersService);
  paginatedOrders: PaginatedOrder | null = null;

  ngOnInit() {
    this.ordersService.getOrders(1, 4)
      .subscribe(orders => this.paginatedOrders = orders);
  }

  handlePageChanged(pageNo: number) {
    this.ordersService.getOrders(pageNo, 4)
      .subscribe(orders => this.paginatedOrders = orders);
  }
}
