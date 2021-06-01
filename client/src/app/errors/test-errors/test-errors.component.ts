import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.scss']
})
export class TestErrorsComponent implements OnInit {
  baseUrl = "https://localhost:44320/api/"
  validationErrors:string[] = [];
  constructor(private http:HttpClient) { }

  ngOnInit(): void {
  }

  get404Error(){
    this.http.get(this.baseUrl+'buggy/GetNotFound').subscribe(res=>{console.log(res)},error=>{
      console.log(error)
    })
  }
  get400Error(){
    this.http.get(this.baseUrl+'buggy/GetBadRequest').subscribe(res=>{console.log(res)},error=>{
      console.log(error)
    })
  }  
  get500Error(){
    this.http.get(this.baseUrl+'buggy/GetServerError').subscribe(res=>{console.log(res)},error=>{
      console.log(error)
    })
  }  
  get401Error(){
    this.http.get(this.baseUrl+'buggy/auth').subscribe(res=>{console.log(res)},error=>{
      console.log(error)
    })
  }
  get400ValidationError(){
    this.http.post(this.baseUrl+'account/Register',{}).subscribe(res=>{console.log(res)},error=>{
      this.validationErrors = error;
      console.log(error)
    })
  }

}
