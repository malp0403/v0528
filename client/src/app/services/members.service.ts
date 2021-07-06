import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, take, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { PaginatedResult } from '../models/pagination';
import { User } from '../models/user';
import { UserParams } from '../models/userParams';
import { AccountService } from './account.service';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();
  memberCache = new Map();
  user:User;
  userParams:UserParams;
  constructor(private http: HttpClient,private accountService:AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user= user;
      this.userParams = new UserParams(user);
    })
   }

   getUserParams(){return this.userParams;}
   setUserParams(params:UserParams){this.userParams = params;}
   resetUserParams(){this.userParams = new UserParams(this.user); return this.userParams;}

  getMembers(userParams:UserParams) {
    var val = [...this.memberCache.values()].reduce((x,y)=>x.concat(y.result),[]);
    console.log('cache:',val);

    const key = Object.values(userParams).join('-');
    var response = this.memberCache.get(key);
    if(response){
      return of(response);
    }
    let params = getPaginationHeaders(userParams.pageNumber,userParams.pageSize);

    params = params.append('minAge',userParams.minAge.toString());
    params = params.append('maxAge',userParams.maxAge.toString());
    params = params.append('gender',userParams.gender);
    params = params.append('orderBy',userParams.orderBy);

    return getPaginatedResult<Member[]>(this.baseUrl +'users/GetUsers',params,this.http).pipe(
      tap(response=>{
        this.memberCache.set(key,response);
      })
    )
  }



  getMember(username: string) {
    // const member = this.members.find(x => x.username == username)
    // if (member) return of(member);
    var val = [...this.memberCache.values()].reduce((x,y)=>x.concat(y.result),[]);
    const member = val.find(x=>x.username == username);
    if(member){
      return of(member);
    }
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

  deletePhoto(photoId: number) {
    return this.http.post(this.baseUrl + 'users/DeletePhoto/' + photoId, {});
  }

  addLike(username:string){
    return this.http.post(this.baseUrl+'likes/AddLike/'+username,{});
  }

  getLikes(predicate:string,pageNumber:number,pageSize:number){
    let params = getPaginationHeaders(pageNumber,pageSize);
    params = params.append('predicate',predicate);

    return getPaginatedResult<Partial<Member[]>>(this.baseUrl+'likes/GetUserLikes',params,this.http);

    // return this.http.get<Partial<Member[]>>(this.baseUrl+'likes/GetUserLikes?predicate='+predicate);
  }


}
