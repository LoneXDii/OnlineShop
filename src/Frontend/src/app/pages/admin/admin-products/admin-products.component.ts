import {Component, inject, OnInit} from '@angular/core';
import {ProductsService} from '../../../data/services/products.service';
import {PaginatedProducts} from '../../../data/interfaces/catalog/paginatedProducts.interface';
import {ProductListItemComponent} from './product-list-item/product-list-item.component';
import {PaginationComponent} from '../../common/pagination/pagination.component';

@Component({
  selector: 'app-admin-products',
  imports: [
    ProductListItemComponent,
    PaginationComponent
  ],
  templateUrl: './admin-products.component.html',
  styleUrl: './admin-products.component.css'
})
export class AdminProductsComponent implements OnInit {
  productsService = inject(ProductsService);
  products?: PaginatedProducts;

  ngOnInit() {
    this.productsService.getProducts(undefined, undefined, undefined, undefined, undefined, 20)
      .subscribe(val => this.products = val);
  }

  handlePageChanged(pageNo: number) {
    this.productsService.getProducts(undefined, undefined, undefined, undefined, pageNo, 20)
      .subscribe(val => this.products = val);
  }
}
