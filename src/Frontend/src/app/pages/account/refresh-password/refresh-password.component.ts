import {Component, inject} from '@angular/core';
import {ProfileService} from '../../../data/services/profile.service';
import {Router} from '@angular/router';
import {AbstractControl, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators} from '@angular/forms';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-refresh-password',
  imports: [
    NgIf,
    ReactiveFormsModule,
  ],
  templateUrl: './refresh-password.component.html',
  styleUrl: './refresh-password.component.css'
})
export class RefreshPasswordComponent {
  profileService = inject(ProfileService);
  router = inject(Router);

  emailForm = new FormGroup({
    email: new FormControl<string | null>(null, [Validators.required, Validators.email])
  });

  resetForm = new FormGroup({
    code: new FormControl<string | null>(null, Validators.required),
    password: new FormControl<string | null>(null, [Validators.required, Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}')]),
    confirmPassword: new FormControl<string | null>(null, Validators.required)
  }, { validators: this.passwordMatchValidator });

  emailSubmitted = false;
  errorMessage: string | null = null;

  passwordMatchValidator(): ValidatorFn {
    return (form: AbstractControl): { [key: string]: any } | null => {
      const password = form.get('password')?.value;
      const confirmPassword = form.get('confirmPassword')?.value;
      return password === confirmPassword ? null : { mismatch: true };
    };
  }

  onEmailSubmit() {
    if (this.emailForm.valid) {
      const email = this.emailForm.value.email;
      //@ts-ignore
      this.profileService.askForPasswordRefreshCode(email).subscribe( {
        next: () => {
          this.emailSubmitted = true;
          this.errorMessage = null;
        },
        error: (error) => {
          this.errorMessage = 'Wrong email. No user found with this email.';
        }
      });
    }
  }

  onResetSubmit() {
    if (this.resetForm.valid) {
      const { password, code } = this.resetForm.value;
      //@ts-ignore
      this.profileService.refreshPassword({ password, code })
        .subscribe({
          next: () => this.router.navigate(['/login']),
          error: (error) => {
            this.errorMessage = 'Wrong code.';
          }
        });
    }
  }
}
