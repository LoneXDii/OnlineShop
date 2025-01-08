import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {PaginatedProducts} from '../interfaces/catalog/paginatedProducts.interface';
import {catchError, of} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  http = inject(HttpClient);
  baseUrl = 'http://localhost:5000';

  getProducts(categoryId?: number, minPrice?: number, maxPrice?: number, ValuesIds?: number[], PageNo?: number, PageSize: number = 10) {
    const params: any = {
      PageSize: PageSize,
    };

    if(categoryId) {
      params.categoryId = categoryId;
    }

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

    return this.http.get<PaginatedProducts>(`${this.baseUrl}/products`, {params: params})
      .pipe(
        catchError(error => {
          if (error.status === 404) {
            return of(undefined);
          }
          throw error;
        })
      );
  }
}
