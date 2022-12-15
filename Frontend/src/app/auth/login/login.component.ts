import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {UserDto} from "../../../interfaces/userDto";
import {LoginService} from "../../../services/login.service";
import jwtDecode from "jwt-decode";
import {Token} from "../../../interfaces/token";
import {Router, RouterModule} from "@angular/router";
import {customAxios, reloadAxios} from "../../../services/httpAxios";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  }, {updateOn: 'submit'});

  collapsed = true;

  constructor(private fb: FormBuilder,
              private loginService: LoginService,
              private router: Router) {
  }

  ngOnInit(): void {
  }

  async login() {
    const dto: UserDto = {
      username: <string>this.loginForm.get("username")?.value,
      password: <string>this.loginForm.get("password")?.value,
    };
    var token = await this.loginService.login(dto);
    localStorage.setItem('token', token)
    reloadAxios();
    let deToken = jwtDecode(token) as Token;
    if (deToken) {
      if (deToken.role == "admin") {
        await this.router.navigate(["/admin/patterns"]);
      } else if (deToken.role == "user") {
        await this.router.navigate(["/user/home"]);
      }
    }
  }
}
