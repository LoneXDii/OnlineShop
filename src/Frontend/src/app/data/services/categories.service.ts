import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {AttributeAllValues} from '../interfaces/catalog/attributeAllValues.interface';
import {Category} from '../interfaces/catalog/category.interface';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/categories`;

  getCategories() {
    return this.http.get<Category[]>(`${this.baseUrl}`);
  }

  getCategoryById(categoryId: number){
    return this.http.get<Category>(`${this.baseUrl}/${categoryId}`);
  }

  getCategoryAttributesValues(categoryId:number) {
    return this.http.get<AttributeAllValues[]>(`${this.baseUrl}/${categoryId}/attributes/values`);
  }

  createCategory(formData: FormData){
    return this.http.post(`${this.baseUrl}`, formData);
  }

  createChildCategory(category: {parentId: number, name: string}) {
    return this.http.post(`${this.baseUrl}/${category.parentId}/attributes`, {name: category.name});
  }

  updateCategory(categoryId: number, formData: FormData) {
    return this.http.put(`${this.baseUrl}/${categoryId}`, formData);
  }

  deleteCategory(categoryId: number) {
    return this.http.delete(`${this.baseUrl}/${categoryId}`);
  }
}
