import {Component, inject, OnInit} from '@angular/core';
import {ProductsService} from '../../../../data/services/products.service';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {Product} from '../../../../data/interfaces/catalog/product.interface';
import {NgForOf, NgIf} from '@angular/common';
import {AuthService} from '../../../../data/services/auth.service';
import {CartService} from '../../../../data/services/cart.service';
import {
  ProductDiscountCreationComponent
} from '../components/product-discount-creation/product-discount-creation.component';

@Component({
  selector: 'app-product-info',
  imports: [
    NgIf,
    NgForOf,
    RouterLink,
    ProductDiscountCreationComponent
  ],
  templateUrl: './product-info.component.html',
  styleUrl: './product-info.component.css'
})
export class ProductInfoComponent implements OnInit {
  productsService = inject(ProductsService);
  authService = inject(AuthService);
  cartService = inject(CartService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  product: Product | null = null;
  isDiscountCreating: boolean = false;

  ngOnInit() {
    this.route.params.subscribe(params => {
      const productId = +params['id'];
      this.refreshProduct(productId);
    });
  }

  refreshProduct(productId: number) {
    this.productsService.getProductById(productId)
      .subscribe(product => {
        this.product = product;
      });
  }

  onDeleteDiscount() {
    if(!this.product || !this.product.discount){
      return;
    }

    return this.productsService.deleteDiscount(this.product.discount.id)
      .subscribe(() => {
        if(this.product){
          this.refreshProduct(this.product.id)
        }
      });
  }

  onDelete(){
    if(!this.product){
      return;
    }

    this.productsService.deleteProduct(this.product.id)
      .subscribe(() => {
        alert('Product deleted successfully.');
        this.router.navigate(['/admin']);
      });
  }

  addToCart(){
    if(!this.product){
      return;
    }

    this.cartService.addProduct(this.product.id, 1)
      .subscribe({
        error: () => alert("This product is out of stock")
      });
  }

  onAddDiscount(){
    this.isDiscountCreating = !this.isDiscountCreating;
  }

  protected readonly AuthService = AuthService;
}
