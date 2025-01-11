import {Component, EventEmitter, inject, Input, OnChanges, OnInit, Output} from '@angular/core';
import {UserWithRoles} from '../../../../data/interfaces/admin/userWithRoles.interface';
import {UsersService} from '../../../../data/services/users.service';
import {AuthService} from '../../../../data/services/auth.service';

@Component({
  selector: 'app-user-list-item',
  imports: [],
  templateUrl: './user-list-item.component.html',
  styleUrl: './user-list-item.component.css'
})
export class UserListItemComponent implements OnInit, OnChanges {
  @Input() user!: UserWithRoles;
  @Output() userUpdated = new EventEmitter<void>();
  usersService = inject(UsersService);
  authService = inject(AuthService);

  ngOnInit(){
    if(this.user.roles.length === 0){
      this.user.roles.push('customer')
    }
  }

  ngOnChanges(){
    if(this.user.roles.length === 0){
      this.user.roles.push('customer')
    }
  }

  onRoleChanged(){
    if(this.user.roles.includes('admin')){
      this.usersService.deleteFromAdmins(this.user.id)
        .subscribe(() => this.userUpdated.emit());
    }
    else{
      this.usersService.makeAdmin(this.user.id)
        .subscribe(() => this.userUpdated.emit());
    }
  }
}
