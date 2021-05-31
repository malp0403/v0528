import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  loginForm:FormGroup;
  model:any= {};
  loggedIn:boolean;
  constructor(private fb:FormBuilder,public accountService:AccountService) {
   }

  ngOnInit(): void {

    this.loginForm = this.fb.group({
      username:[null,[Validators.required]],
      password:[null,[Validators.required]]
    })
  }

  login(){
    const {username,password}= this.loginForm.getRawValue();
    const model = {
      username,password
    };

    this.accountService.login(model).subscribe(res=>{
      console.log(res);
    },error=>{
      console.log(error)
    })
  }
  
  logout(){
    this.accountService.logout();
  }

}
