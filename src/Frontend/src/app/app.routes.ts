import { Routes } from '@angular/router';
import {CatalogComponent} from './pages/catalog/catalog.component';
import {CategoriesComponent} from './pages/categories/categories.component';
import {LoginComponent} from './pages/account/login/login.component';
import {canActivateAuth} from './data/guards/auth.guard';
import {ProfileComponent} from './pages/profile/profile.component';
import {RegisterComponent} from './pages/account/register/register.component';
import {RefreshPasswordComponent} from './pages/account/refresh-password/refresh-password.component';
import {canActivateUnauthenticated} from './data/guards/alreadyAuthenticated.guard';
import {ConfirmEmailComponent} from './pages/account/confirm-email/confirm-email.component';
import {AdminComponent} from './pages/admin/admin.component';
import {canActivateAdmin} from './data/guards/admin.guard';
import {
  ProductCreationComponent
} from './pages/cruds/product/product-creation/product-creation.component';
import {ProductEditingComponent} from './pages/cruds/product/product-editing/product-editing.component';
import {ProductInfoComponent} from './pages/cruds/product/product-info/product-info.component';
import {CategoryCreationComponent} from './pages/cruds/category/category-creation/category-creation.component';
import {CategoryEditingComponent} from './pages/cruds/category/category-editing/category-editing.component';
import {CategoryInfoComponent} from './pages/cruds/category/category-info/category-info.component';

export const routes: Routes = [
  {path: 'catalog/:categoryId', component: CatalogComponent},
  {path: 'catalog', component: CategoriesComponent},
  {path: 'login', component: LoginComponent, canActivate: [canActivateUnauthenticated]},
  {path: 'register', component: RegisterComponent, canActivate: [canActivateUnauthenticated]},
  {path: 'confirm-email/:email', component: ConfirmEmailComponent},
  {path: 'refresh-password', component: RefreshPasswordComponent, canActivate: [canActivateUnauthenticated]},
  {path: 'profile', component: ProfileComponent, canActivate: [canActivateAuth]},
  {path: 'admin', component: AdminComponent, canActivate: [canActivateAdmin]},
  {path: 'admin/:tab', component: AdminComponent, canActivate: [canActivateAdmin]},
  {path: 'products/:id/info', component: ProductInfoComponent},
  {path: 'products/create', component: ProductCreationComponent, canActivate: [canActivateAdmin]},
  {path: 'products/:id/edit', component: ProductEditingComponent, canActivate: [canActivateAdmin]},
  {path: 'categories/:id/info', component: CategoryInfoComponent, canActivate: [canActivateAdmin]},
  {path: 'categories/create', component: CategoryCreationComponent, canActivate: [canActivateAdmin]},
  {path: 'categories/:id/edit', component: CategoryEditingComponent, canActivate: [canActivateAdmin]},
];
