import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginInputModel, LoginService } from 'app/core';

@Component({
  selector: 'login',
  templateUrl: './login-component.html'
})

export class LoginComponent implements OnInit {

  public loginForm: FormGroup;

  constructor(private loginService: LoginService) {

  }
  ngOnInit(): void {
    this.buildForm();
  }

  buildForm = () => {
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });

  }

  onFormSubmit = (formValue) => {
    if (!this.loginForm.valid) {
      return;
    }

    let userLoginData = <LoginInputModel>{
      Username: formValue.username,
      Password: formValue.password,
    };

    this.loginService.login(userLoginData).subscribe(res => {
      console.log(res);
    });
  }
}
