import {Component, inject, OnInit} from '@angular/core';
import {RouterLink} from '@angular/router';
import {AuthService} from '../../../data/services/auth.service';
import {ProfileInfoComponent} from './profile-info/profile-info.component';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-navigation',
  imports: [
    RouterLink,
    ProfileInfoComponent,
    NgIf
  ],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.css'
})
export class NavigationComponent implements OnInit {
  isLoggedIn: boolean = false;
  isAdmin: boolean = false;
  authService = inject(AuthService);

  ngOnInit() {
    this.authService.loggedIn$.subscribe(status => {
      this.isLoggedIn = status;
    });
    this.authService.isAdmin$.subscribe(status => {
      this.isAdmin = status;
    })
  }
}
