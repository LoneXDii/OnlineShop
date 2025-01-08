import {Component, inject, OnInit} from '@angular/core';
import {RouterLink} from '@angular/router';
import {AuthService} from '../../../data/services/auth.service';
import {ProfileInfoComponent} from './profile-info/profile-info.component';

@Component({
  selector: 'app-navigation',
  imports: [
    RouterLink,
    ProfileInfoComponent
  ],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.css'
})
export class NavigationComponent implements OnInit {
  isLoggedIn: boolean = false;
  authService = inject(AuthService);

  ngOnInit() {
    this.authService.loggedIn$.subscribe(status => {
      this.isLoggedIn = status;
    })
  }
}
