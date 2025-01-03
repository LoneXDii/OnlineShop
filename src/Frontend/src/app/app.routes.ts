import { Routes } from '@angular/router';
import {CatalogComponent} from './pages/catalog/catalog.component';
import {CategoriesComponent} from './pages/categories/categories.component';
import {LoginComponent} from './pages/login/login.component';
import {canActivateAuth} from './data/guards/auth.guard';

export const routes: Routes = [
  {path: 'catalog/:categoryId', component: CatalogComponent},
  {path: 'catalog', component: CategoriesComponent},
  {path: 'login', component: LoginComponent},
  //TEMPORARY
  {path: 'account', component: CatalogComponent, canActivate: [canActivateAuth]},
];
