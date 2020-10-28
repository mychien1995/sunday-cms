import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import {
  LoginService,
  AuthenticationService,
  ClientState,
  LayoutService,
} from '@services/index';
import { LoginInputModel, LoginResponseModel } from '@models/index';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login-component.html',
})
export class LoginComponent implements OnInit {
  public loginForm: FormGroup;

  constructor(
    private loginService: LoginService,
    private authService: AuthenticationService,
    private clientState: ClientState,
    private router: Router,
    private toastr: ToastrService,
    private layoutService: LayoutService
  ) {}

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/']);
    }
    this.buildForm();
  }

  buildForm = () => {
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });
  };

  onFormSubmit = (formValue) => {
    if (!this.loginForm.valid) {
      return;
    }

    const userLoginData = <LoginInputModel>{
      Username: formValue.username,
      Password: formValue.password,
    };
    this.clientState.isBusy = true;
    this.loginService.login(userLoginData).subscribe(
      (res) => {
        this.toastr.success('Welcome', null, {
          positionClass: 'toast-bottom-right',
        });
        const userData = <LoginResponseModel>res;
        this.authService.storeUserData(userData);
        this.layoutService.layoutUpdated({
          event: 'user-updated',
        });
        this.layoutService.refresh(() => {
          this.clientState.isBusy = false;
          this.router.navigate(['/']);
        });
      },
      (err) => {
        this.clientState.isBusy = false;
        if (err.error.Errors) {
          this.toastr.error(err.error.Errors);
        } else {
          this.toastr.error('Invalid username or password');
        }
      }
    );
  };
}
