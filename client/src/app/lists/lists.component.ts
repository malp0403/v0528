import { Component, OnInit } from '@angular/core';
import { Member } from '../models/member';
import { Pagination } from '../models/pagination';
import { MembersService } from '../services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.scss']
})
export class ListsComponent implements OnInit {
  members:Partial<Member[]>;
  predicate = 'liked';
  pageNumebr = 1;
  pageSize = 1;
  pagination:Pagination;
  constructor(private membersService:MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes(){
    this.membersService.getLikes(this.predicate,this.pageNumebr,this.pageSize).subscribe(response=>{
      this.members= response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event:any){
    this.pageNumebr = event.page;
    this.loadLikes();
  }

}
