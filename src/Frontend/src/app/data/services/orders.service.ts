import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {CartService} from './cart.service';
import {tap} from 'rxjs';
import {PaginatedOrder} from '../interfaces/cart/paginetedOrder.interface';
import {Order} from '../interfaces/cart/order.interface';
import {AuthService} from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  http = inject(HttpClient);
  cartService = inject(CartService);
  baseUrl = `${environment.apiUrl}/orders`;
  authService = inject(AuthService);


  getOrders(pageNo:number = 1, pageSize:number = 10) {
    const params = {pageNo:pageNo, pageSize:pageSize};

    return this.http.get<PaginatedOrder>(`${this.baseUrl}`, {params: params});
  }

  getAllOrders(pageNo:number = 1, pageSize:number = 10) {
    const params = {pageNo:pageNo, pageSize:pageSize};

    return this.http.get<PaginatedOrder>(`${this.baseUrl}/admin`, {params: params});
  }

  getOrderById(id: string) {
    if(this.authService.isAdmin){
      return this.http.get<Order>(`${this.baseUrl}/${id}/admin`);
    }

    return this.http.get<Order>(`${this.baseUrl}/${id}`);
  }

  getUsersOrders(userId: string, pageNo:number = 1, pageSize:number = 10) {
    const params = {pageNo:pageNo, pageSize:pageSize};

    return this.http.get<PaginatedOrder>(`http://localhost:5000/users/${userId}/orders`, {params: params});
  }

  createOrder(){
    return this.http.post(`${this.baseUrl}`, {})
      .pipe(
        tap(() => this.cartService.loadCartInfo())
      );
  }

  confirmOrder(orderId: string){
    return this.http.put(`${this.baseUrl}/${orderId}/confirmation/admin`, {});
  }

  completeOrder(orderId: string) {
    return this.http.put(`${this.baseUrl}/${orderId}/completion/admin`, {});
  }

  cancelOrder(orderId: string){
    if(this.authService.isAdmin){
      return this.http.put(`${this.baseUrl}/${orderId}/cancellation/admin`, {});
    }

    return this.http.put(`${this.baseUrl}/${orderId}/cancellation`, {});
  }
}
