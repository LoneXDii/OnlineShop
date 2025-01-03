import {Component, inject, Input, OnInit} from '@angular/core';
import {ProductsService} from '../../data/services/products.service';
import {PaginatedProducts} from '../../data/interfaces/paginatedProducts.interface';
import {Category} from '../../data/interfaces/category.interface';
import {NavigationComponent} from '../common/navigation/navigation.component';
import {PaginationComponent} from '../common/pagination/pagination.component';
import {ProductCardsLayoutComponent} from './components/product-cards-layout/product-cards-layout.component';
import {SidebarComponent} from './components/sidebar/sidebar.component';

@Component({
  selector: 'app-catalog',
  imports: [
    NavigationComponent,
    PaginationComponent,
    ProductCardsLayoutComponent,
    SidebarComponent
  ],
  templateUrl: './catalog.component.html',
  styleUrl: './catalog.component.css'
})
export class CatalogComponent implements OnInit {
  @Input() category!: Category;

  productsService = inject(ProductsService);
  products?: PaginatedProducts;

  minPrice?: number;
  maxPrice?: number;
  valuesIds?: number[];

  ngOnInit() {
    this.productsService.getProducts(this.category.id, undefined, undefined, undefined)
      .subscribe(val => this.products = val);
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
