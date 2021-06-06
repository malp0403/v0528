import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  constructor(private http: HttpClient) { }

  getMembers() {
    if (this.members.length > 0) return of(this.members);
    return this.http.get<Member[]>(this.baseUrl + "users/GetUsers").pipe(
      tap(members => {
        this.members = members;
      })
    )
  }

  getMember(username: string) {
    const member = this.members.find(x => x.username == username)
    if (member) return of(member);
    return this.http.get<Member>(this.baseUrl + "users/GetUser/" + username)
  }

  updateMember(member: Member) {
    return this.http.post(this.baseUrl + 'users/UpdateUser', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }

  setMainPhoto(photoId: number) {
    return this.http.post(this.baseUrl + 'users/SetMainPhoto/' + photoId, {});
  }

  deletePhoto(photoId:number){
    return this.http.post(this.baseUrl + 'users/DeletePhoto/' + photoId, {});
  }
}
