import {Component, OnInit} from '@angular/core';
import jwtDecode from "jwt-decode";
import {Token} from "../../interfaces/token";
import {debounceTime} from "rxjs";
import {Router} from "@angular/router";
import {UserService} from "../../services/user.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.sass']
})
export class HeaderComponent implements OnInit {
  userid: any;
  user: any;

  constructor(private router: Router, private userService: UserService) {

  }

  username: string = "default"

  async ngOnInit(){
    let t = localStorage.getItem("token");
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.username = deToken.username;
      this.userid = deToken.userId
    }

    this.user = await this.userService.getUserById(this.userid)
  }
}
