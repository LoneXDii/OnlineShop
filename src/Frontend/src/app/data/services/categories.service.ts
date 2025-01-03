import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {AttributeAllValues} from '../interfaces/catalog/attributeAllValues.interface';
import {Category} from '../interfaces/catalog/category.interface';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {
  http = inject(HttpClient);
  baseUrl = 'http://localhost:5000/categories';

  constructor() { }

  getCategories() {
    return this.http.get<Category[]>(`${this.baseUrl}`);
  }

  getCategoryById(categoryId: number){
    return this.http.get<Category>(`${this.baseUrl}/${categoryId}`);
  }

  getCategoryAttributesValues(categoryId:number) {
    return this.http.get<AttributeAllValues[]>(`${this.baseUrl}/${categoryId}/attributes/values`);
  }
}
