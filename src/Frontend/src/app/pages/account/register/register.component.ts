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
    password: new FormControl<string | null>(null, [Validators.required, Validators.minLength(6)]),
    avatar: new FormControl<File | null>(null)
  });

  onFileChange(event: any) {
    const file = event.target.files[0];
    this.form.patchValue({
      avatar: file
    });
  }

  onSubmit() {
    console.log("Entered");
    if (this.form.valid) {
      console.log("OK");
      console.log(this.form.value);

      const formData = new FormData();
      //@ts-ignore
      formData.append('firstName', this.form.get('firstName')?.value);
      //@ts-ignore
      formData.append('lastName', this.form.get('lastName')?.value);
      //@ts-ignore
      formData.append('email', this.form.get('email')?.value);
      //@ts-ignore
      formData.append('password', this.form.get('password')?.value);
      //@ts-ignore
      formData.append('avatar', this.form.get('avatar')?.value);

      this.authService.register(formData)
        .subscribe({
          next: () => {
            console.log('User registered successfully');
            this.router.navigate(['/login']);
          },
          error: (err) => {
            console.error('Registration error', err);
          }
      });
    }
  }
}
