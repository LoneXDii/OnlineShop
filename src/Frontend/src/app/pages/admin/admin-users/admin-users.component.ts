import {Component, inject, OnInit} from '@angular/core';
import {UsersService} from '../../../data/services/users.service';
import {PaginatedUsers} from '../../../data/interfaces/admin/paginatedUsers.interface';
import {PaginationComponent} from '../../common/pagination/pagination.component';
import {UserListItemComponent} from './user-list-item/user-list-item.component';

@Component({
  selector: 'app-admin-users',
  imports: [
    PaginationComponent,
    UserListItemComponent
  ],
  templateUrl: './admin-users.component.html',
  styleUrl: './admin-users.component.css'
})
export class AdminUsersComponent implements OnInit {
  usersService = inject(UsersService);
  currentPage: number = 1;
  users?: PaginatedUsers;

  ngOnInit() {
    this.refreshUsers();
  }

  handlePageChanged(pageNo: number) {
    this.currentPage = pageNo;
    this.refreshUsers();
  }

  refreshUsers(){
    this.usersService.getUsers(this.currentPage, 20)
      .subscribe(val => this.users = val);
  }
}
