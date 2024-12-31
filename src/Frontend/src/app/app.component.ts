import {Component, inject} from '@angular/core';
import {ProductsService} from './data/services/products.service';
import {PaginatedProducts} from './data/interfaces/paginatedProducts.interface';
import {
  ProductCardsLayoutComponent
} from './pages/catalog/components/product-cards-layout/product-cards-layout.component';
import {NavigationComponent} from './pages/common/navigation/navigation.component';
import {SidebarComponent} from './pages/catalog/components/sidebar/sidebar.component';
import {Category} from './data/interfaces/category.interface';

@Component({
  selector: 'app-root',
  imports: [ProductCardsLayoutComponent, NavigationComponent, SidebarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Frontend';
  productsService = inject(ProductsService);
  products!: PaginatedProducts;

  category: Category = {
    id: 1,
    name: 'Test',
    imageUrl: 'https://example.com/image.jpg'
  };

  constructor() {
    this.productsService.getProducts()
      .subscribe(val => this.products = val);
  }
}
