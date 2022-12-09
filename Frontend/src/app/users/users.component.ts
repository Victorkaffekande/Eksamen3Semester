import { Component, OnInit } from '@angular/core';
import {UserService} from "../../services/user.service";
import jwtDecode from "jwt-decode";
import {Token} from "../../interfaces/token";

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.sass']
})
export class UsersComponent implements OnInit {
  searchText: any;
  users: any;

  constructor(private userService: UserService) { }

  async ngOnInit(){

    this.users = await this.userService.getAllUsers()
  }

}
