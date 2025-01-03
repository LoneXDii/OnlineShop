import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {AttributeAllValues} from '../interfaces/attributeAllValues.interface';
import {Category} from '../interfaces/category.interface';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {
  http = inject(HttpClient);
  baseUrl = 'http://localhost:5000/categories';

  constructor() { }

  getCategoryById(categoryId: number){
    return this.http.get<Category>(`${this.baseUrl}/${categoryId}`);
  }

  getCategoryAttributesValues(categoryId:number) {
    return this.http.get<AttributeAllValues[]>(`${this.baseUrl}/${categoryId}/attributes/values`);
  }
}
