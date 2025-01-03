import {Component, inject, Input, OnInit} from '@angular/core';
import {ProductsService} from '../../data/services/products.service';
import {PaginatedProducts} from '../../data/interfaces/catalog/paginatedProducts.interface';
import {Category} from '../../data/interfaces/catalog/category.interface';
import {PaginationComponent} from '../common/pagination/pagination.component';
import {ProductCardsLayoutComponent} from './components/product-cards-layout/product-cards-layout.component';
import {SidebarComponent} from './components/sidebar/sidebar.component';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-catalog',
  imports: [
    PaginationComponent,
    ProductCardsLayoutComponent,
    SidebarComponent
  ],
  templateUrl: './catalog.component.html',
  styleUrl: './catalog.component.css'
})
export class CatalogComponent implements OnInit {
  categoryId!: number;

  productsService = inject(ProductsService);
  products?: PaginatedProducts;

  minPrice?: number;
  maxPrice?: number;
  valuesIds?: number[];

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.categoryId = +params['categoryId']; // Преобразуем в число, если нужно
      this.fetchProducts({minPrice: this.minPrice, maxPrice: this.maxPrice, valuesIds: this.valuesIds});
    });
  }

  fetchProducts(params: { minPrice?: number; maxPrice?: number; valuesIds?: number[] }){
    this.minPrice = params.minPrice;
    this.maxPrice = params.maxPrice;
    this.valuesIds = params.valuesIds;

    this.productsService.getProducts(this.categoryId, this.minPrice, this.maxPrice, this.valuesIds)
      .subscribe(val => this.products = val);
  }

  handlePageChanged(pageNo: number) {
    this.productsService.getProducts(this.categoryId, this.minPrice, this.maxPrice, this.valuesIds, pageNo)
      .subscribe(val => this.products = val);
  }
}
