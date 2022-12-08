import { Component, OnInit } from '@angular/core';
import {PatternService} from "../../services/pattern.service";
import {LikeService} from "../../services/like.service";
import jwtDecode from "jwt-decode";
import {Token} from "../../interfaces/token";

@Component({
  selector: 'app-liked-users',
  templateUrl: './liked-users.component.html',
  styleUrls: ['./liked-users.component.sass']
})
export class LikedUsersComponent implements OnInit {
   userId: any;

  constructor(private likeService: LikeService) { }
  users: any;
  searchText: any;

  async ngOnInit() {
    let t = localStorage.getItem("token");
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.userId = deToken.userId;
    }

    this.users = await this.likeService.getAllLikedUsersByUser(this.userId)
  }

}
