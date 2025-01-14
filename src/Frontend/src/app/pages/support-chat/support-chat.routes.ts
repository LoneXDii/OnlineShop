import {Routes} from '@angular/router';
import {AdminChatsComponent} from './admin-chats/admin-chats.component';
import {canActivateAdmin} from '../../data/guards/admin.guard';

export const supportChatRoutes: Routes = [
  {path: 'support/admin', component: AdminChatsComponent, canActivate: [canActivateAdmin]},
]
