import {Component, inject} from '@angular/core';
import {ProfileService} from '../../data/services/profile.service';
import {Profile} from '../../data/interfaces/auth/profile.interface';

@Component({
  selector: 'app-profile',
  imports: [],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  profileService = inject(ProfileService);
  profile!: Profile;

  constructor() {
    this.profileService.getUserInfo()
      .subscribe(val => this.profile = val);
  }
}
