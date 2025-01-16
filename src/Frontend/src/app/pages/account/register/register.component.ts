import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../../data/services/auth.service';
import {NgIf} from '@angular/common';
import {tap} from 'rxjs';

@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule,
    NgIf,
    RouterLink
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  authService = inject(AuthService);
  router = inject(Router);

  form = new FormGroup({
    firstName: new FormControl<string | null>(null, Validators.required),
    lastName: new FormControl<string | null>(null, Validators.required),
    email: new FormControl<string | null>(null, [Validators.required, Validators.email]),
    password: new FormControl<string | null>(null, [Validators.required, Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}')]),
    avatar: new FormControl<File | null>(null)
  });

  onFileChange(event: any) {
    const file = event.target.files[0];
    this.form.patchValue({
      avatar: file
    });
  }

  onSubmit() {
    if (!this.form.valid) {
      return;
    }

    const formData = new FormData();

    formData.append('firstName', this.form.get('firstName')?.value || '');
    formData.append('lastName', this.form.get('lastName')?.value || '');
    formData.append('email', this.form.get('email')?.value || '');
    formData.append('password', this.form.get('password')?.value || '');
    formData.append('avatar', this.form.get('avatar')?.value || '');

    this.authService.register(formData)
      .subscribe({
        next: () => {
          this.router.navigate([`/confirm-email/${formData.get('email')}`]);
        },
        error: (err) => {
          console.error('Registration error', err);
        }
      });
  }
}
