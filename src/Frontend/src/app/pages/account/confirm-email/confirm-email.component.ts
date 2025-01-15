import {Component, inject, OnInit} from '@angular/core';
import { AuthService } from '../../../data/services/auth.service';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-confirm-email',
  imports: [
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css'
})
export class ConfirmEmailComponent implements OnInit {
  authService = inject(AuthService);
  route = inject(ActivatedRoute);
  router = inject(Router);

  email: string | null = null;
  errorMessage: string | null = null;

  form = new FormGroup({
    code: new FormControl<string | null>(null, [
      Validators.required,
      Validators.minLength(6),
      Validators.maxLength(6)
    ])
  });

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.email = params['email'];
    });
  }

  confirmEmail() {
    if (!this.form.valid) {
      return;
    }

    const code = this.form.value.code;

    if (!code){
      return;
    }

    this.authService.confirmEmail({ email: this.email!, code })
      .subscribe({
        next: () => {
          this.router.navigate(['/login']);
        },
        error: () => {
          this.errorMessage = 'Invalid confirmation code. Please try again.';
        }
      });
  }

  resendEmailConfirmationCode() {
    if(this.email) {
      this.authService.resendEmailConfirmationCode(this.email)
        .subscribe(() => alert("Your code was resend successfully."));
    }
  }
}
