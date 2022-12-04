import { Component, OnInit } from '@angular/core';
import jwtDecode from "jwt-decode";
import {Token} from "../../../interfaces/token";
import {UserService} from "../../../services/user.service";
import {FormBuilder, Validators} from "@angular/forms";
import {NgbDate} from "@ng-bootstrap/ng-bootstrap";

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

  async submitEdit() {
    var x = <NgbDate><unknown>this.formGroup.get("birthday")?.value;
    console.log(x)
    let day = String(x.day);
    if (x.day < 10) {
      day = '0' + day
    }

    let month = String(x.month);
    if (x.month < 10) {
      month = '0' + month
    }

    let dateString = x.year + "-" + month + "-" + day;

    let dto = {
      username: <string><unknown>this.formGroup.get('username')?.value,
      id: this.userId,
      email: <string><unknown>this.formGroup.get('email')?.value,
      birthday: dateString,
      profilePicture: this.user.profilePicture,

    }

    await this.userService.updateUser(dto)
      .then(() =>
        window.location.reload()
      )
      .catch(error => {
        this.errorMsg = error.response.data;
      });

  }

  cancelEdit() {
    window.location.reload()
  }

  //convert selected image to a blob image
  onFileSelectedImage($event: Event) {
    // @ts-ignore
    const file: File = event.target.files[0];
    const reader: FileReader = new FileReader();
    reader.onloadend = (e) => {
      this.user.profilePicture = reader.result;
    }
    if (file) {
      reader.readAsDataURL(file)
    }
  }

}
