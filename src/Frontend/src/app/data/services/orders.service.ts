import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Order} from '../interfaces/cart/order.interface';
import {CartService} from './cart.service';
import {tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  http = inject(HttpClient);
  cartService = inject(CartService);
  baseUrl = `${environment.apiUrl}/orders`;

  getOrders(){
    return this.http.get<Order[]>(`${this.baseUrl}`);
  }

  createOrder(){
    return this.http.post(`${this.baseUrl}`, {})
      .pipe(
        tap(() => this.cartService.loadCartInfo())
      );
  }
}
