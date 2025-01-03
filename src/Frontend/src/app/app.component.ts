import {Component, inject} from '@angular/core';
import {ProductsService} from './data/services/products.service';
import {PaginatedProducts} from './data/interfaces/paginatedProducts.interface';
import {
  ProductCardsLayoutComponent
} from './pages/catalog/components/product-cards-layout/product-cards-layout.component';
import {NavigationComponent} from './pages/common/navigation/navigation.component';
import {SidebarComponent} from './pages/catalog/components/sidebar/sidebar.component';
import {Category} from './data/interfaces/category.interface';
import {PaginationComponent} from './pages/common/pagination/pagination.component';

@Component({
  selector: 'app-root',
  imports: [ProductCardsLayoutComponent, NavigationComponent, SidebarComponent, PaginationComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Frontend';
  productsService = inject(ProductsService);
  products?: PaginatedProducts;

  minPrice?: number;
  maxPrice?: number;
  valuesIds?: number[];

  category: Category = {
    id: 1,
    name: 'Test',
    imageUrl: 'https://example.com/image.jpg'
  };

  constructor() {
    this.productsService.getProducts(this.category.id, undefined, undefined, undefined)
      .subscribe(val => {
        this.products = val
        console.log(val)
      });
  }

  fetchProducts(params: { minPrice?: number; maxPrice?: number; valuesIds?: number[] }){
    this.minPrice = params.minPrice;
    this.maxPrice = params.maxPrice;
    this.valuesIds = params.valuesIds;

    this.productsService.getProducts(this.category.id, this.minPrice, this.maxPrice, this.valuesIds)
      .subscribe(val => this.products = val);
  }

  handlePageChanged(pageNo: number) {
    this.productsService.getProducts(this.category.id, this.minPrice, this.maxPrice, this.valuesIds, pageNo)
      .subscribe(val => this.products = val);
  }
}
