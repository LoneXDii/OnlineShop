import {Component, inject, OnInit} from '@angular/core';
import {CategoriesService} from '../../../../data/services/categories.service';
import {ActivatedRoute, Router} from '@angular/router';
import {Category} from '../../../../data/interfaces/catalog/category.interface';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {NgIf} from '@angular/common';
import {CategoryFormComponent} from '../components/category-form/category-form.component';

@Component({
  selector: 'app-category-editing',
  imports: [
    NgIf,
    CategoryFormComponent
  ],
  templateUrl: './category-editing.component.html',
  styleUrl: './category-editing.component.css'
})
export class CategoryEditingComponent implements OnInit {
  categoriesService = inject(CategoriesService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  categoryId: number | null = null;
  category: Category | null = null;

  form = new FormGroup({
    name: new FormControl<string | null>(null, Validators.required),
    image: new FormControl<File | null>(null),
  });

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.categoryId = +params['id'];
      this.categoriesService.getCategoryById(this.categoryId)
        .subscribe(category => {
          this.category = category;
          this.form.patchValue({
            name: category.name,
          });
        });
    });
  }

  onSubmit(formData: FormData) {
    if(this.categoryId != null){
      this.categoriesService.updateCategory(this.categoryId, formData)
        .subscribe(() => this.router.navigate(['/admin']));
    }
  }
}
