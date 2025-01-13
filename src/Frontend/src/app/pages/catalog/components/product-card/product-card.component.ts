import {Component, inject, Input} from '@angular/core';
import {Product} from '../../../../data/interfaces/catalog/product.interface';
import {RouterLink} from '@angular/router';
import { CartService } from '../../../../data/services/cart.service';
import {AuthService} from '../../../../data/services/auth.service';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-product-card',
  imports: [
    RouterLink,
    NgIf
  ],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.css'
})
export class ProductCardComponent {
  @Input() product!: Product;
  cartService = inject(CartService);
  authService = inject(AuthService);

  addToCart(){
    this.cartService.addProduct(this.product.id, 1)
      .subscribe({
        error: () => alert("This product is out of stock")
      });
  }
}
