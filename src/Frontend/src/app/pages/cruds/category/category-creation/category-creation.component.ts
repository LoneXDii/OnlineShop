import {Component, inject} from '@angular/core';
import {CategoriesService} from '../../../../data/services/categories.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {CategoryFormComponent} from '../components/category-form/category-form.component';
import {Router} from '@angular/router';

@Component({
  selector: 'app-category-creation',
  imports: [
    CategoryFormComponent
  ],
  templateUrl: './category-creation.component.html',
  styleUrl: './category-creation.component.css'
})
export class CategoryCreationComponent {
  categoriesService = inject(CategoriesService);
  router = inject(Router);

  form = new FormGroup({
    name: new FormControl<string | null>(null, Validators.required),
    image: new FormControl<File | null>(null),
  });

  onSubmit(formData: FormData) {
    this.categoriesService.createCategory(formData)
      .subscribe(() => this.router.navigate(['/admin']));
  }
}
