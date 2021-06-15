import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
  maxDate:Date;
  registerForm: FormGroup;
  validationErrors:string[] = [];
  constructor(private fb: FormBuilder, private accountService: AccountService, private toastr: ToastrService,private router:Router) { }

  usernameMsg = {
    'required': 'this is required',
    'minlength': 'at least 3'
  }
  passwordMsg = {
    'required': 'this is required',
    'isMatching': 'password doesnt match'
  }

  ngOnInit(): void {
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear()-18);
    this.registerForm = this.fb.group({
      username: [null, [Validators.required, Validators.minLength(3)]],
      gender: ['male'],
      knownAs: ['', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      city: ['', [Validators.required]],
      country: ['', [Validators.required]],
      password: [null, [Validators.required]],
      confirmPassword: [null, [Validators.required, this.matchValues('password')]]
    })
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
    this.registerForm.valueChanges.subscribe(res => {
      console.log(this.registerForm);
    })
  }

  register() {
    // const { username, password } = this.registerForm.getRawValue();
    // const user = {
    //   username, password
    // }
    const user = this.registerForm.getRawValue()
    this.accountService.register(user).subscribe(res => {
      this.router.navigateByUrl('/members');
    }, error => {
      this.validationErrors = error;
      this.toastr.error(error.error);
    });


  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value == control.parent?.controls[matchTo].value ? null : { isMatching: true };
    }
  }

  cancel() {
    this.cancaleRegister.emit(false);
  }
}
