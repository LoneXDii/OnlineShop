import {CatalogComponent} from './catalog.component';
import {CategoriesComponent} from '../categories/categories.component';
import {Routes} from '@angular/router';

export const catalogRoutes: Routes = [
  {path: 'catalog/:categoryId', component: CatalogComponent},
  {path: 'catalog', component: CategoriesComponent},
  {path: '', component: CategoriesComponent},
]
