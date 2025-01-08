import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Profile} from '../interfaces/auth/profile.interface';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
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
}
