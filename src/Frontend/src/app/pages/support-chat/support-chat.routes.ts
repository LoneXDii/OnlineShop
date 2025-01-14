import {Routes} from '@angular/router';
import {AdminChatsComponent} from './admin-chats/admin-chats.component';
import {canActivateAdmin} from '../../data/guards/admin.guard';
import {UserChatsComponent} from './user-chats/user-chats.component';
import {canActivateAuth} from '../../data/guards/auth.guard';
import {ChatComponent} from './chat/chat.component';

export const supportChatRoutes: Routes = [
  {path: 'support/admin', component: AdminChatsComponent, canActivate: [canActivateAdmin]},
  {path: 'support', component: UserChatsComponent, canActivate: [canActivateAuth]},
  {path: 'support/chat/:id', component: ChatComponent, canActivate: [canActivateAuth]},
]
