import {AdminComponent} from './admin.component';
import {canActivateAdmin} from '../../data/guards/admin.guard';
import {Routes} from '@angular/router';

export const adminRoutes: Routes = [
  {path: 'admin', component: AdminComponent, canActivate: [canActivateAdmin]},
  {path: 'admin/:tab', component: AdminComponent, canActivate: [canActivateAdmin]}
]
