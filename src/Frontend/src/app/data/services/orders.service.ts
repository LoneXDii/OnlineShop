import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {CartService} from './cart.service';
import {tap} from 'rxjs';
import {PaginatedOrder} from '../interfaces/cart/paginetedOrder.interface';
import {Order} from '../interfaces/cart/order.interface';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  http = inject(HttpClient);
  cartService = inject(CartService);
  baseUrl = `${environment.apiUrl}/orders`;

  getOrders(pageNo:number = 1, pageSize:number = 10) {
    const params = {pageNo:pageNo, pageSize:pageSize};

    return this.http.get<PaginatedOrder>(`${this.baseUrl}`, {params: params});
  }

  getOrderById(id: string) {
    return this.http.get<Order>(`${this.baseUrl}/${id}`);
  }

  createOrder(){
    return this.http.post(`${this.baseUrl}`, {})
      .pipe(
        tap(() => this.cartService.loadCartInfo())
      );
  }
}
