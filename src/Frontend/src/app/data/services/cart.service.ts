import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject} from 'rxjs';
import {Cart} from '../interfaces/cart/cart.interface';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  http = inject(HttpClient);
  baseUrl = 'http://localhost:5000/cart';

  private cartItemsCount = new BehaviorSubject<number>(0);
  cartItemsCount$ = this.cartItemsCount.asObservable();

  private cartTotalCost = new BehaviorSubject<number>(0);
  cartTotalCost$ = this.cartTotalCost.asObservable();

  constructor() {
    this.loadCartInfo();
  }

  private loadCartInfo(){
    this.http.get<Cart>(`${this.baseUrl}`)
      .subscribe({
        next: (cart) => {
          this.cartItemsCount.next(cart.count);
          this.cartTotalCost.next(cart.totalCost);
        },
        error: () => {
          this.cartItemsCount.next(0);
          this.cartTotalCost.next(0);
        }
      })
  }
}
