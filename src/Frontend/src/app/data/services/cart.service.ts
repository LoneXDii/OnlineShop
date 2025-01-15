import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject, tap} from 'rxjs';
import {Cart} from '../interfaces/cart/cart.interface';
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/cart`;

  private cartItemsCount = new BehaviorSubject<number>(0);
  cartItemsCount$ = this.cartItemsCount.asObservable();

  private cartTotalCost = new BehaviorSubject<number>(0);
  cartTotalCost$ = this.cartTotalCost.asObservable();

  constructor() {
    this.loadCartInfo();
  }

  loadCartInfo(){
    this.getCart()
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

  getCart(){
    return this.http.get<Cart>(`${this.baseUrl}`);
  }

  increaseProductQuantity(productId: number, quantity: number){
    return this.http.post(`${this.baseUrl}/products/${productId}/quantity`, {quantity: quantity})
        .pipe(
            tap(() => this.loadCartInfo())
        );
  }

  decreaseProductQuantity(productId: number){
    return this.http.patch(`${this.baseUrl}/products/${productId}/quantity`, {quantity: 1})
        .pipe(
            tap(() => this.loadCartInfo())
        );
  }

  addProduct(productId: number, quantity: number){
    return this.http.post(`${this.baseUrl}/products`, {id: productId, quantity: quantity})
        .pipe(
            tap(() => this.loadCartInfo())
        );
  }

  removeProduct(productId: number){
    return this.http.delete(`${this.baseUrl}/products/${productId}`)
        .pipe(
            tap(() => this.loadCartInfo())
        );
  }

  clearCart(){
    this.http.delete(`${this.baseUrl}`)
        .subscribe(() => this.loadCartInfo());
  }
}
