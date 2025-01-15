import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {PaginatedProducts} from '../interfaces/catalog/paginatedProducts.interface';
import {catchError, of} from 'rxjs';
import {Product} from '../interfaces/catalog/product.interface';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  http = inject(HttpClient);
  baseUrl = `${environment.apiUrl}/products`;

  createProduct(formData: FormData){
    return this.http.post(`${this.baseUrl}`, formData);
  }

  updateProduct(productId: number, formData: FormData){
    return this.http.put(`${this.baseUrl}/${productId}`, formData);
  }

  deleteProduct(productId: number){
    return this.http.delete(`${this.baseUrl}/${productId}`);
  }

  getProductById(id: number){
    return this.http.get<Product>(`${this.baseUrl}/${id}`);
  }

  getProducts(options: {
    categoryId?: number;
    minPrice?: number;
    maxPrice?: number;
    valuesIds?: number[];
    pageNo?: number;
    pageSize?: number;
  } = {}) {
    const params: any = {
      PageSize: options.pageSize ?? 10,
      ...Object.entries(options)
        .filter(([_, value]) => value && true)
        .reduce((res, [key, value]) => {
          res[key] = value;
          return res;
        }, {} as any)
    };

    return this.http.get<PaginatedProducts>(`${this.baseUrl}`, { params })
      .pipe(
        catchError(error => {
          if (error.status === 404) {
            return of(undefined);
          }
          throw error;
        })
      );
  }

  addDiscount(params: {productId: number, percent: number, endDate: Date}) {
    return this.http.post(`${environment.apiUrl}/discounts`, {...params, startDate: new Date(Date.now() + 120)});
  }

  deleteDiscount(discountId: number) {
    return this.http.delete(`${environment.apiUrl}/discounts/${discountId}`);
  }
}
