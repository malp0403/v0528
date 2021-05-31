import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from '../models/user';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  @Input() users: any;
  @Output() cancaleRegister = new EventEmitter();
  model: any = {};
  registerForm: FormGroup;
  constructor(private fb: FormBuilder, private accountService: AccountService) { }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      username: [null, [Validators.required]],
      password: [null, [Validators.required]]
    })
  }

  register() {
    const { username, password } = this.registerForm.getRawValue();
    const user = {
      username, password
    }
    this.accountService.register(user).subscribe(res => {
      console.log(res);
      this.cancel();
    },error=>{
      console.log(error);
    });
  }

  cancel() {
    this.cancaleRegister.emit(false);
  }
}
