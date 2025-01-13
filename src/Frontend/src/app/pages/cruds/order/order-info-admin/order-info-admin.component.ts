import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {OrdersService} from '../../../../data/services/orders.service';
import {Order} from '../../../../data/interfaces/cart/order.interface';
import {NgIf} from '@angular/common';
import {
  OrderInfoTemplateComponent
} from '../../../orders/order-info/components/order-info-template/order-info-template.component';
import {OrderUserInfoComponent} from './components/order-user-info/order-user-info.component';

@Component({
  selector: 'app-order-info-admin',
  imports: [
    NgIf,
    OrderInfoTemplateComponent,
    OrderUserInfoComponent
  ],
  templateUrl: './order-info-admin.component.html',
  styleUrl: './order-info-admin.component.css'
})
export class OrderInfoAdminComponent implements OnInit{
  route = inject(ActivatedRoute);
  orderService = inject(OrdersService);
  order: Order | null = null;
  errorMessage: string | null = null;

  ngOnInit() {
    this.route.params.subscribe(params => {
      const orderId = params['id'];
      this.orderService.getOrderByIdAsAdmin(orderId)
        .subscribe({
          next: order => {
            this.order = order;
            this.errorMessage = null;
          },
          error: () => {
            this.order = null;
            this.errorMessage = 'Order not found. Please check the order ID and try again. If the problem persists, contact support.';
          }});
    })
  }
}
