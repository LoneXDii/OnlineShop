import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {AttributeAllValues} from '../interfaces/attributeAllValues.interface';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {
  http = inject(HttpClient);
  baseUrl = 'http://localhost:5000';

  constructor() { }

  getCategoryAttributesValues(categoryId:number) {
    return this.http.get<AttributeAllValues[]>(`${this.baseUrl}/categories/${categoryId}/attributes/values`);
  }
}
