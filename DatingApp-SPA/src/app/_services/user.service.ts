import { Injectable } from '@angular/core';
import { environment} from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseURL = environment.apiUrl;

constructor(private http: HttpClient) { }
    getUsers(): Observable<User[]> {
        return this.http.get<User[]>(this.baseURL + 'users/');
    }
    getUser(id): Observable<User> {
      return this.http.get<User>(this.baseURL + 'users/' + id);
  }

   updateUser(id: number, user: User) {
    return this.http.put(this.baseURL + 'users/' + id, user);
}

setMainPhoto(userId: number, id: number) {
  return this.http.post(this.baseURL + 'users/' + userId + '/photos/' + id +  '/setMain', {});
}

deletePhoto(userId: number, id: number) {
  return this.http.delete(this.baseURL + 'users/' + userId + '/photos/' + id);
}

}
