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

  getProducts(){
    return this.http.get<PaginatedProducts>(`${this.baseUrl}/products`);
  }
}
