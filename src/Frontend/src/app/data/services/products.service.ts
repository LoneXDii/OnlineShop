import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {PaginatedProducts} from '../interfaces/paginatedProducts.interface';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  http = inject(HttpClient);
  baseUrl = 'http://localhost:5000';

  constructor() { }

  getProducts(categoryId: number, minPrice?: number, maxPrice?: number, ValuesIds?: number[], PageNo?: number) {
    const params: any = {
      CategoryId: categoryId,
    };

    if (minPrice) {
      params.MinPrice = minPrice;
    }

    if (maxPrice) {
      params.MaxPrice = maxPrice;
    }

    if (ValuesIds) {
      params.ValuesIds = ValuesIds;
    }

    if (PageNo) {
      params.PageNo = PageNo;
    }

    return this.http.get<PaginatedProducts>(`${this.baseUrl}/products`, {params: params});
  }
}
