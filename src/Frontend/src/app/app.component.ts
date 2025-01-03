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
  products?: PaginatedProducts;

  category: Category = {
    id: 1,
    name: 'Test',
    imageUrl: 'https://example.com/image.jpg'
  };

  constructor() {
    this.productsService.getProducts(this.category.id, undefined, undefined, undefined)
      .subscribe(val => this.products = val);
  }

  fetchProducts(params: { minPrice?: number; maxPrice?: number; ValuesIds?: number[] }){
    console.log("entered")
    console.log(this)
    this.productsService.getProducts(this.category.id, params.minPrice, params.maxPrice, params.ValuesIds)
      .subscribe(val => this.products = val);
  }
}
