import { Routes } from '@angular/router';
import {CatalogComponent} from './pages/catalog/catalog.component';
import {CategoriesComponent} from './pages/categories/categories.component';
import {LoginComponent} from './pages/account/login/login.component';
import {canActivateAuth} from './data/guards/auth.guard';
import {ProfileComponent} from './pages/profile/profile.component';
import {RegisterComponent} from './pages/account/register/register.component';

export const routes: Routes = [
  {path: 'catalog/:categoryId', component: CatalogComponent},
  {path: 'catalog', component: CategoriesComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'profile', component: ProfileComponent, canActivate: [canActivateAuth]},
];
