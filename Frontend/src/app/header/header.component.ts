import {Component, OnInit} from '@angular/core';
import jwtDecode from "jwt-decode";
import {Token} from "../../interfaces/token";
import {debounceTime} from "rxjs";
import {ActivatedRoute, Event, NavigationEnd, Router} from "@angular/router";
import {UserService} from "../../services/user.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.sass']
})
export class HeaderComponent implements OnInit {
  userid: any;
  user: any;
  page: string | null = "";
  profilePicture: any

  constructor(private router: Router,  private userService: UserService) {


  }

  username: string = "default"
  panelOpenState: boolean | undefined;


  async ngOnInit(){
    let t = localStorage.getItem("token");
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.username = deToken.username;
      this.userid = deToken.userId
    }

    this.user = await this.userService.getUserById(this.userid).then(u => this.profilePicture = u.profilePicture)
  }
  logout() {
    localStorage.removeItem('token');
    this.router.navigate([''])
  }
}
