import {Component, inject} from '@angular/core';
import {ProductsService} from './data/services/products.service';
import {PaginatedProducts} from './data/interfaces/paginatedProducts.interface';
import {
  ProductCardsLayoutComponent
} from './pages/catalog/components/product-cards-layout/product-cards-layout.component';

@Component({
  selector: 'app-root',
  imports: [ProductCardsLayoutComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Frontend';
  productsService = inject(ProductsService);
  products!: PaginatedProducts;

  constructor() {
    this.productsService.getProducts()
      .subscribe(val => this.products = val);
  }
}
