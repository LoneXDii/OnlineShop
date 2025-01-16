import {Component, inject, Input, OnInit} from '@angular/core';
import {UsersService} from '../../../../../../data/services/users.service';
import {Profile} from '../../../../../../data/interfaces/auth/profile.interface';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-order-user-info',
  imports: [
    NgIf
  ],
  templateUrl: './order-user-info.component.html',
  styleUrl: './order-user-info.component.css'
})
export class OrderUserInfoComponent implements OnInit {
  @Input() userId!: string;
  usersService = inject(UsersService);
  user: Profile | null = null;

  ngOnInit() {
    this.usersService.getUserById(this.userId)
      .subscribe(user => this.user = user);
  }
}
