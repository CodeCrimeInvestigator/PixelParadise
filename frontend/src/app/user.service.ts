import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import { Observable } from 'rxjs';

interface User {
  id: string;
  userName: string;
  nickName: string;
  email: string;
  age: number;
}

interface UserResponse {
  items: User[];
  pageSize: number;
  page: number;
  total: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = 'http://localhost:5000/users'; // Backend URL

  constructor(private http: HttpClient) {}

  getUsers(page: number, pageSize: number): Observable<UserResponse> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<UserResponse>(this.apiUrl, { params });
  }
}
