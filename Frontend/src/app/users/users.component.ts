import {Component, OnInit} from '@angular/core';
import {UserService} from "../../services/user.service";
import jwtDecode from "jwt-decode";
import {Token} from "../../interfaces/token";
import {filter} from "rxjs";

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.sass']
})
export class UsersComponent implements OnInit {
  searchText: any;
  users: any;
  userId: any;

  constructor(private userService: UserService) {
  }

  async ngOnInit() {
    this.setupList();
  }

  async setupList() {
    this.getUserId();
    this.users = await this.userService.getAllUsers()
    this.users = this.users.filter((u: { id: any }) => u.id != this.userId);
  }

  getUserId() {
    let t = localStorage.getItem('token');
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.userId = deToken.userId;
    }

  }
}
