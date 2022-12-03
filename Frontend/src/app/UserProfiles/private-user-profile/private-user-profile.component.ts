import { Component, OnInit } from '@angular/core';
import jwtDecode from "jwt-decode";
import {Token} from "../../../interfaces/token";
import {UserService} from "../../../services/user.service";
import {FormBuilder, Validators} from "@angular/forms";

@Component({
  selector: 'app-private-user-profile',
  templateUrl: './private-user-profile.component.html',
  styleUrls: ['./private-user-profile.component.sass']
})
export class PrivateUserProfileComponent implements OnInit {
  private userId: any;
user: any;
  constructor(private userService: UserService, private _formBuilder: FormBuilder) { }





  async ngOnInit(){

    let t = localStorage.getItem("token");
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.userId = deToken.userId;
    }

    this.user = await this.userService.getUserById(this.userId)

    this.fillForm();
  }




  formGroup = this._formBuilder.group({
    username: [{value: ''}],
    birthday: [{value: ''}],
    email : [{value: ''}],
    profilePicture: [{value: ''}],
  });
  errorMsg: any;


  fillForm() {
    if (this.user) {
      this.formGroup.controls['username'].setValue(this.user.username);
      this.formGroup.controls['birthday'].setValue(this.user.birthDay);
      this.formGroup.controls['email'].setValue(this.user.email);
      this.formGroup.controls['profilePicture'].setValue(this.user.profilePicture);
    }
  }

  submitEdit() {

  }

  cancelEdit() {

  }

  onFileSelectedImage($event: Event) {

  }
}
