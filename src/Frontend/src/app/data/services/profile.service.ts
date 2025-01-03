import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Profile} from '../interfaces/auth/profile.interface';
import {tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  http = inject(HttpClient);
  baseUrl = 'http://localhost:5000/users';

  constructor() { }

  getUserInfo(){
    return this.http.get<Profile>(`${this.baseUrl}/info`)
      .pipe(
        tap(val => console.log(val))
      )
  }
}
