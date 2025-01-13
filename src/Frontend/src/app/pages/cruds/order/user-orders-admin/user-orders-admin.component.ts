import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {PaginatedOrder} from '../../../../data/interfaces/cart/paginetedOrder.interface';
import {OrdersService} from '../../../../data/services/orders.service';
import {NgIf} from '@angular/common';
import {
  OrdersListItemComponent
} from '../../../orders/orders-list/components/orders-list-item/orders-list-item.component';
import {PaginationComponent} from '../../../common/pagination/pagination.component';
import {OrderUserInfoComponent} from '../order-info-admin/components/order-user-info/order-user-info.component';

@Component({
  selector: 'app-user-orders-admin',
  imports: [
    NgIf,
    OrdersListItemComponent,
    PaginationComponent,
    OrderUserInfoComponent
  ],
  templateUrl: './user-orders-admin.component.html',
  styleUrl: './user-orders-admin.component.css'
})
export class UserOrdersAdminComponent implements OnInit {
  ordersService = inject(OrdersService);
  route = inject(ActivatedRoute);
  paginatedOrders: PaginatedOrder | null = null;
  userId: string | null = null;
  errorMessage: string | null = null;


  ngOnInit() {
    this.route.params.subscribe(
      params => {
        this.userId = params['id'];

        if(!this.userId) {
          return;
        }

        this.ordersService.getUsersOrders(this.userId, 1, 4)
          .subscribe({
            next: orders => {
              this.paginatedOrders = orders;
              this.errorMessage = null;
            },
            error: () => {
              this.paginatedOrders = null;
              this.errorMessage = "No orders found for this user";
            }
          });
      }
    )
  }

  handlePageChanged(pageNo: number) {
    if(!this.userId) {
      return;
    }

    this.ordersService.getUsersOrders(this.userId, pageNo, 4)
      .subscribe(orders => this.paginatedOrders = orders);
  }
}
