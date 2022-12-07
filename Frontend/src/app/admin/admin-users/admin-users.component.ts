import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.sass']
})
export class AdminUsersComponent implements OnInit {

  constructor() {
  }

  //TODO LAV EN GETALL USERS OG FYLD DENNE LISTE
  userList: any = [{
    birthDay: "2022-12-07",
    email: "fillerUser@mail.com",
    id: 1,
    profilePicture: null,
    username: "fillerUser"
  }];

  //Todo laven get all admins til at fill list
  adminList: any = [{
    birthDay: "2022-12-07",
    email: "ADMINLMAO@mail.com",
    id: 2,
    profilePicture: null,
    username: "MR BIG ADMIN"
  }];

  ngOnInit(): void {

  }

  deleteUser(id: any) {
//TODO USERSERVICE
  }
}
