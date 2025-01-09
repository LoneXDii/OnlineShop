import {Component, inject, OnInit} from '@angular/core';
import {ProductsService} from '../../../../data/services/products.service';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {Product} from '../../../../data/interfaces/catalog/product.interface';
import {NgForOf, NgIf} from '@angular/common';
import {AuthService} from '../../../../data/services/auth.service';

@Component({
  selector: 'app-product-info',
  imports: [
    NgIf,
    NgForOf,
    RouterLink
  ],
  templateUrl: './product-info.component.html',
  styleUrl: './product-info.component.css'
})
export class ProductInfoComponent implements OnInit {
  productsService = inject(ProductsService);
  authService = inject(AuthService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  product: Product | null = null;

  ngOnInit() {
    this.route.params.subscribe(params => {
      const productId = +params['id'];
      this.productsService.getProductById(productId)
        .subscribe(product => {
          this.product = product;
        })
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
}
