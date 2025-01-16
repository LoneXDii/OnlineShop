import {Component, inject, Input} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {ProfileService} from '../../../data/services/profile.service';
import {Router} from '@angular/router';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-profile-email',
  imports: [
    FormsModule,
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './profile-email.component.html',
  styleUrl: './profile-email.component.css'
})
export class ProfileEmailComponent {
  @Input() email!: string;
  profileService = inject(ProfileService);
  router = inject(Router);

  editingEmail = false;

  emailForm = new FormGroup({
    email: new FormControl<string | null>(null, [Validators.required, Validators.email])
  });

  editEmail() {
    this.emailForm.patchValue({
      email: this.email
    });

    this.editingEmail = !this.editingEmail;
  }

  submitEmail() {
    if (!this.emailForm.valid || !this.emailForm.value.email) {
      return;
    }

    const email = this.emailForm.value.email;

    this.profileService.updateEmail({email: email})
      .subscribe({
        next: () =>{
          this.email = email;
          this.editingEmail = false;
          this.router.navigate([`/confirm-email/${this.emailForm.value.email}`]);
        },
        error: (err) => {
          console.error('Update error', err);
        }
      });
  }
}
