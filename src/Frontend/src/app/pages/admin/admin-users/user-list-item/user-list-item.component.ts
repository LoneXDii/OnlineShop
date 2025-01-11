import {Component, Input, OnInit} from '@angular/core';
import {UserWithRoles} from '../../../../data/interfaces/admin/userWithRoles.interface';

@Component({
  selector: 'app-user-list-item',
  imports: [],
  templateUrl: './user-list-item.component.html',
  styleUrl: './user-list-item.component.css'
})
export class UserListItemComponent implements OnInit {
  @Input() user!: UserWithRoles;

  ngOnInit(){
    if(this.user.roles.length === 0){
      this.user.roles.push('customer')
    }
  }
}
