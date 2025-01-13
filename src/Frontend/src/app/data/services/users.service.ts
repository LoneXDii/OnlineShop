import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {PaginatedUsers} from '../interfaces/admin/paginatedUsers.interface';
import {environment} from '../../../environments/environment';
import {Profile} from '../interfaces/auth/profile.interface';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  http = inject(HttpClient);
  baseUrl = `${environment.apiUrl}/users`;

  getUsers(pageNo:number = 1, pageSize:number = 10) {
    const params = {pageNo:pageNo, pageSize:pageSize};

    return this.http.get<PaginatedUsers>(`${this.baseUrl}`, {params: params});
  }

  getUserById(id: string) {
    return this.http.get<Profile>(`${this.baseUrl}/${id}/info`);
  }

  makeAdmin(userId: string){
    return this.http.post(`${this.baseUrl}/${userId}/roles/admin`, {});
  }

  deleteFromAdmins(userId: string){
    return this.http.delete(`${this.baseUrl}/${userId}/roles/admin`);
  }
}
