import {Component, EventEmitter, inject, Output} from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidatorFn,
  Validators
} from '@angular/forms';
import {ProfileService} from '../../../data/services/profile.service';
import {AuthService} from '../../../data/services/auth.service';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-profile-password',
  imports: [
    FormsModule,
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './profile-password.component.html',
  styleUrl: './profile-password.component.css'
})
export class ProfilePasswordComponent {
  @Output() changePassword = new EventEmitter<void>();
  profileService = inject(ProfileService);
  authService = inject(AuthService);

  passwordForm = new FormGroup({
    oldPassword: new FormControl<string | null>(null, Validators.required),
    newPassword: new FormControl<string | null>(null, [Validators.required, Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}')]),
    repeatPassword: new FormControl<string | null>(null, Validators.required)
  }, { validators: this.passwordMatchValidator });

  passwordMatchValidator(): ValidatorFn {
    return (form: AbstractControl): { [key: string]: any } | null => {
      const password = form.get('password')?.value;
      const confirmPassword = form.get('confirmPassword')?.value;
      return password === confirmPassword ? null : { mismatch: true };
    };
  }

  submitPassword() {
    if (!this.passwordForm.valid) {
      return;
    }

    const { oldPassword, newPassword } = this.passwordForm.value;

    if(!(oldPassword && newPassword)) {
      return;
    }

    this.profileService.updatePassword({ oldPassword, newPassword })
      .subscribe({
        next: (val) => {
          this.passwordForm.reset();
          this.authService.refreshToken = val;
          alert('Password changed successfully!');
        },
        error: () => {
          console.error('Password change error');
        }
      });
  }

  onCancelChanging(){
   this.changePassword.emit();
  }
}
