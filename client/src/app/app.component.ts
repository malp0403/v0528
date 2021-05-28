import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'client';
  users:any;
  constructor(private http:HttpClient){

  }

  ngOnInit(): void {
    this.getUsers();
  }
  
  getUsers(){
    this.http.get('https://localhost:44320/api/users/GetUsers').subscribe(res=>{
      this.users= res;
      console.log(res)
    })
  }
}
