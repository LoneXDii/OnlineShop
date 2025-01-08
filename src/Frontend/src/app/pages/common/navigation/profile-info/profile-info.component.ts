import {Component, inject, OnInit} from '@angular/core';
import {AuthService} from '../../../../data/services/auth.service';
import {jwtDecode} from 'jwt-decode';
import {DecodedToken} from '../../../../data/interfaces/auth/decodedToken.interface';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-profile-info',
  imports: [
    RouterLink
  ],
  templateUrl: './profile-info.component.html',
  styleUrl: './profile-info.component.css'
})
export class ProfileInfoComponent implements OnInit {
  authService = inject(AuthService);
  email: string | null = null;
  avatarUrl: string | null = null;

  ngOnInit() {
    const decodedToken = jwtDecode<DecodedToken>(this.authService.getAccessToken);
    this.email = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
    this.avatarUrl = decodedToken.Avatar;
  }

  handleLogout() {
    this.authService.logout();
  }
}
