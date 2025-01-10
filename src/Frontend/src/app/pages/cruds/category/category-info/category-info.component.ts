import {Component, inject, OnInit} from '@angular/core';
import {AuthService} from '../../../../data/services/auth.service';
import {CategoriesService} from '../../../../data/services/categories.service';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {Category} from '../../../../data/interfaces/catalog/category.interface';
import {NgIf} from '@angular/common';
import {
  CategoryAttributesValuesComponent
} from '../components/category-attributes-values/category-attributes-values.component';

@Component({
  selector: 'app-category-info',
  imports: [
    NgIf,
    CategoryAttributesValuesComponent,
    RouterLink
  ],
  templateUrl: './category-info.component.html',
  styleUrl: './category-info.component.css'
})
export class CategoryInfoComponent implements OnInit {
  categoriesService = inject(CategoriesService);
  authService = inject(AuthService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  categoryId: number | null = null;
  category: Category | null = null;

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.categoryId = +params['id'];
      this.refreshCategory();
    });
  }

  refreshCategory(){
    if(!this.categoryId){
      return;
    }
    this.categoriesService.getCategoryById(this.categoryId)
      .subscribe(category => this.category = category);
  }

  onDelete() {
    if(!this.categoryId){
      return;
    }
    this.categoriesService.deleteCategory(this.categoryId)
      .subscribe({
        next: () => this.refreshCategory(),
        error: err => alert("Can't delete category that is in use by any product")
      });
  }
}
