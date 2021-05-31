import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users:any;
  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle(){
    this.registerMode = true;
  }

  getUsers(){
    this.http.get('https://localhost:44320/api/Users/GetUsers').subscribe(users=>{
      console.log(users);
      this.users= users
    });
  }

  cancel(e:any){
    this.registerMode = e;
  }

}
