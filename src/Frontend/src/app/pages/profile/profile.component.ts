import {Component, inject} from '@angular/core';
import {ProfileService} from '../../data/services/profile.service';
import {Profile} from '../../data/interfaces/auth/profile.interface';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {NgIf} from '@angular/common';
import {AuthService} from '../../data/services/auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-profile',
  imports: [
    ReactiveFormsModule,
    NgIf
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  profileService = inject(ProfileService);
  authService = inject(AuthService);
  router = inject(Router);
  profile!: Profile;

  editingProfile = false;
  editingEmail = false;

  profileForm = new FormGroup({
    firstName: new FormControl<string | null>(null, Validators.required),
    lastName: new FormControl<string | null>(null, Validators.required),
    avatar: new FormControl<File | null>(null)
  });

  emailForm = new FormGroup({
    email: new FormControl<string | null>(null, [Validators.required, Validators.email])
  });

  constructor() {
    this.loadProfile();
  }

  loadProfile() {
    this.profileService.getUserInfo()
      .subscribe(val => {
        this.profile = val;
        this.profileForm.patchValue({
          firstName: this.profile.firstName,
          lastName: this.profile.lastName
        });
        this.emailForm.patchValue({
          email: this.profile.email
        });
      });
  }

  editProfile() {
    this.editingProfile = !this.editingProfile;
  }

  submitProfile() {
    if (this.profileForm.valid) {
      const formData = new FormData();
      //@ts-ignore
      formData.append('firstName', this.profileForm.get('firstName')?.value);
      //@ts-ignore
      formData.append('lastName', this.profileForm.get('lastName')?.value);
      //@ts-ignore
      formData.append('avatar', this.profileForm.get('avatar')?.value);

      this.profileService.updateProfile(formData)
        .subscribe({
          next: () =>{
            this.loadProfile();
            this.editingProfile = false;
          },
          error: (err) => {
            console.error('Update error', err);
          }});
    }
  }

  editEmail() {
    this.editingEmail = !this.editingEmail;
  }

  submitEmail() {
    if (this.emailForm.valid) {
      //@ts-ignore
      this.profileService.updateEmail({email: this.emailForm.value.email})
        .subscribe({
          next: () =>{
            //@ts-ignore
            this.profile.email = this.emailForm.value.email;
            this.editingEmail = false;
            this.router.navigate([`/confirm-email/${this.emailForm.value.email}`]);
          },
          error: (err) => {
            console.error('Update error', err);
          }});
    }
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    this.profileForm.patchValue({
      avatar: file
    });
  }

  logout() {
    this.authService.logout();
  }

  changePassword() {
    // some logic
  }
}
