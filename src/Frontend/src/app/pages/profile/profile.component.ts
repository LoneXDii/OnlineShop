import {Component, inject, OnInit} from '@angular/core';
import {ProfileService} from '../../data/services/profile.service';
import {Profile} from '../../data/interfaces/auth/profile.interface';
import {ReactiveFormsModule,} from '@angular/forms';
import {NgIf} from '@angular/common';
import {AuthService} from '../../data/services/auth.service';
import {ProfileUserComponent} from './profile-user/profile-user.component';
import {ProfileEmailComponent} from './profile-email/profile-email.component';
import {ProfilePasswordComponent} from './profile-password/profile-password.component';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-profile',
  imports: [
    ReactiveFormsModule,
    NgIf,
    ProfileUserComponent,
    ProfileEmailComponent,
    ProfilePasswordComponent,
    RouterLink
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  profileService = inject(ProfileService);
  authService = inject(AuthService);
  profile!: Profile;

  changingPassword = false;

  ngOnInit() {
    this.loadProfile();
  }

  loadProfile() {
    this.profileService.getUserInfo()
      .subscribe(val => {
        this.profile = val;
      });
  }

  logout() {
    this.authService.logout();
  }

  changePassword() {
    this.changingPassword = !this.changingPassword;
  }
}
