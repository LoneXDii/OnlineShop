import {Component, inject, OnInit} from '@angular/core';
import {OrdersService} from '../../../data/services/orders.service';
import {PaginatedOrder} from '../../../data/interfaces/cart/paginetedOrder.interface';
import {NgIf} from '@angular/common';
import {AdminOrdersListItemComponent} from './admin-orders-list-item/admin-orders-list-item.component';
import {PaginationComponent} from '../../common/pagination/pagination.component';

@Component({
  selector: 'app-admin-orders',
  imports: [
    NgIf,
    AdminOrdersListItemComponent,
    PaginationComponent
  ],
  templateUrl: './admin-orders.component.html',
  styleUrl: './admin-orders.component.css'
})
export class AdminOrdersComponent implements OnInit {
  ordersService = inject(OrdersService);
  paginatedOrders: PaginatedOrder | null = null;

  ngOnInit() {
    this.ordersService.getAllOrders(1, 10)
      .subscribe(orders => this.paginatedOrders = orders);
  }

  handlePageChanged(pageNo: number) {
    this.ordersService.getAllOrders(pageNo, 10)
      .subscribe(orders => this.paginatedOrders = orders);
  }

  onOrderChanged() {
    if(this.paginatedOrders) {
      this.handlePageChanged(this.paginatedOrders?.currentPage);
    }
  }
}
