import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Profile} from '../interfaces/auth/profile.interface';
import {AuthService} from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  authService = inject(AuthService);
  http = inject(HttpClient);
  baseUrl = 'http://localhost:5000/users';

  constructor() { }

  getUserInfo(){
    return this.http.get<Profile>(`${this.baseUrl}/info`);
  }

  askForPasswordRefreshCode(email: string){
    return this.http.get(`${this.baseUrl}/password/resetting?email=${email}`);
  }

  refreshPassword(params: {password: string, code: string}) {
    return this.http.post(`${this.baseUrl}/password/resetting`, params)
  }

  updateProfile(formData: FormData){
    return this.http.post(`${this.baseUrl}`, formData);
  }

  updateEmail(params:{email: string}) {
    return this.http.put(`${this.baseUrl}/email`, params);
  }

  updatePassword(params: {oldPassword: string, newPassword: string}) {
    return this.http.put(`${this.baseUrl}/password`, params, {responseType: 'text'});
  }
}
