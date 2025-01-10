import {Component, inject, OnInit} from '@angular/core';
import {ProductsService} from '../../../data/services/products.service';
import {PaginatedProducts} from '../../../data/interfaces/catalog/paginatedProducts.interface';
import {ProductListItemComponent} from './product-list-item/product-list-item.component';
import {PaginationComponent} from '../../common/pagination/pagination.component';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-admin-products',
  imports: [
    ProductListItemComponent,
    PaginationComponent,
    RouterLink
  ],
  templateUrl: './admin-products.component.html',
  styleUrl: './admin-products.component.css'
})
export class AdminProductsComponent implements OnInit {
  productsService = inject(ProductsService);
  products?: PaginatedProducts;
  currentPage = 1;

  ngOnInit() {
    this.refreshProducts();
  }

  handlePageChanged(pageNo: number) {
    this.currentPage = pageNo;
    this.refreshProducts();
  }

  refreshProducts(){
    this.productsService.getProducts({
      pageNo: this.currentPage,
      pageSize: 20
    })
      .subscribe(val => this.products = val);
  }
}
