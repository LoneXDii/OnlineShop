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

export const routes: Routes = [
  {path: 'catalog/:categoryId', component: CatalogComponent},
  {path: 'catalog', component: CategoriesComponent},
  {path: 'login', component: LoginComponent, canActivate: [canActivateUnauthenticated]},
  {path: 'register', component: RegisterComponent, canActivate: [canActivateUnauthenticated]},
  {path: 'confirm-email/:email', component: ConfirmEmailComponent},
  {path: 'refresh-password', component: RefreshPasswordComponent, canActivate: [canActivateUnauthenticated]},
  {path: 'profile', component: ProfileComponent, canActivate: [canActivateAuth]},
];
