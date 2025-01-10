import {Component, inject, OnInit} from '@angular/core';
import {AuthService} from '../../../../data/services/auth.service';
import {CategoriesService} from '../../../../data/services/categories.service';
import {ActivatedRoute, Router} from '@angular/router';
import {Category} from '../../../../data/interfaces/catalog/category.interface';
import {NgIf} from '@angular/common';
import {
  CategoryAttributesValuesComponent
} from '../components/category-attributes-values/category-attributes-values.component';

@Component({
  selector: 'app-category-info',
  imports: [
    NgIf,
    CategoryAttributesValuesComponent
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
}
