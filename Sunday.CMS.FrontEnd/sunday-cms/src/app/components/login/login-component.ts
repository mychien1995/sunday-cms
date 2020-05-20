import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService, AuthenticationService, ClientState } from '@services/index';
import { LoginInputModel, LoginResponseModel } from '@models/index';

@Component({
  selector: 'login',
  templateUrl: './login-component.html'
})

export class LoginComponent implements OnInit {

  public loginForm: FormGroup;

  constructor(private loginService: LoginService,
    private authService: AuthenticationService,
    private clientState: ClientState,
    private router: Router) {

  }
  ngOnInit(): void {
    if (this.authService.isAuthenticated())
      this.router.navigate(['/']);
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
    this.clientState.isBusy = true;
    this.loginService.login(userLoginData).subscribe(res => {
      this.clientState.isBusy = false;
      var userData = <LoginResponseModel>(res);
      this.authService.storeUserData(userData);
      this.router.navigate(['/']);
    });
  }
}
