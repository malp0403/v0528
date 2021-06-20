import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/models/member';
import { Pagination } from 'src/app/models/pagination';
import { User } from 'src/app/models/user';
import { UserParams } from 'src/app/models/userParams';
import { AccountService } from 'src/app/services/account.service';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.scss']
})
export class MemberListComponent implements OnInit {
  // members$: Observable<Member[]>;
  members:Member[];
  pagination:Pagination;
  userParams:UserParams;
  user:User;
  genderList=[{value:'male',display:'Males'},{value:'female',display:'Female'}]

  filterForm:FormGroup;

  constructor(private membersService:MembersService,private formBuilder:FormBuilder) { 
    this.userParams = this.membersService.getUserParams();
  }

  ngOnInit(): void {
    this.filterForm=this.formBuilder.group({
      minAge:18,
      maxAge:99,
      gender:this.userParams.gender=='male'?'female':'male'
    })
    this.loadMembers();
  }

  loadMembers(){
    this.membersService.setUserParams(this.userParams);
    this.membersService.getMembers(this.userParams).subscribe(res=>{
      this.members = res.result;
      this.pagination = res.pagination;
    });
  }

  resetFilters(){
    this.userParams = this.membersService.resetUserParams();
    this.filterForm.patchValue({
      minAge:16,
      maxAge:99,
      gender:this.user.gender == 'male'?'female':'male',
    })
    this.loadMembers();
  }

  applyFilter(){
    const values = this.filterForm.getRawValue();
    this.userParams  = {...this.userParams,...values}
    this.loadMembers();
  }

  pageChanged(e:any){
    this.userParams.pageNumber = e.page;
    this.membersService.setUserParams(this.userParams);
    this.loadMembers();
  }
  setOrderby(val:string){
    this.userParams.orderBy = val;
    this.loadMembers();
  }

}
