import {Component, Input} from '@angular/core';
import {Profile} from '../../../../data/interfaces/auth/profile.interface';

@Component({
  selector: 'app-user-list-item',
  imports: [],
  templateUrl: './user-list-item.component.html',
  styleUrl: './user-list-item.component.css'
})
export class UserListItemComponent {
  @Input() user!: Profile;
}
