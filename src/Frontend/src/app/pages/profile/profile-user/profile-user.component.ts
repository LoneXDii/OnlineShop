import {Component, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {Profile} from '../../../data/interfaces/auth/profile.interface';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {ProfileService} from '../../../data/services/profile.service';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-profile-user',
  imports: [
    FormsModule,
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './profile-user.component.html',
  styleUrl: './profile-user.component.css'
})
export class ProfileUserComponent {
  @Input() profile!: Profile;
  @Output() profileUpdated = new EventEmitter<void>();
  profileService = inject(ProfileService);

  profileForm = new FormGroup({
    firstName: new FormControl<string | null>(null, Validators.required),
    lastName: new FormControl<string | null>(null, Validators.required),
    avatar: new FormControl<File | null>(null)
  });

  editingProfile = false;

  editProfile() {
    this.profileForm.patchValue({
      firstName: this.profile.firstName,
      lastName: this.profile.lastName
    });
    this.editingProfile = !this.editingProfile;
  }

  submitProfile() {
    if (!this.profileForm.valid) {
      return;
    }

    const formData = new FormData();

    formData.append('firstName', this.profileForm.get('firstName')?.value || '');
    formData.append('lastName', this.profileForm.get('lastName')?.value || '');
    formData.append('avatar', this.profileForm.get('avatar')?.value || '');

    this.profileService.updateProfile(formData)
      .subscribe({
        next: () =>{
          this.profileUpdated.emit();
          this.editingProfile = false;
        },
        error: (err) => {
          console.error('Update error', err);
        }
      });
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    this.profileForm.patchValue({
      avatar: file
    });
  }
}
