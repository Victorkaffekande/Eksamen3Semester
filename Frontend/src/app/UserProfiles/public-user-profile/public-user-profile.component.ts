import { Component, OnInit } from '@angular/core';
import jwtDecode from "jwt-decode";
import {Token} from "../../../interfaces/token";
import {UserService} from "../../../services/user.service";
import {ActivatedRoute} from "@angular/router";
import {PatternService} from "../../../services/pattern.service";

@Component({
  selector: 'app-public-user-profile',
  templateUrl: './public-user-profile.component.html',
  styleUrls: ['./public-user-profile.component.sass']
})
export class PublicUserProfileComponent implements OnInit {
  user: any;
  userId: any;
  userRouteId:any;
  showEditScreen: boolean = false;
  patterns: any;

  constructor(private patternService: PatternService,private userService: UserService, private activatedRoute: ActivatedRoute) { }


  async ngOnInit(){
    let t = localStorage.getItem("token");
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.userId = deToken.userId;
    }
    this.userRouteId = this.activatedRoute.snapshot.paramMap.get('id');


    this.user = await this.userService.getUserById(this.userRouteId)

    this.patterns = await this.patternService.getPatternsByUserId(this.userRouteId)

  }

  changeSubView() {
    this.showEditScreen = true;
  }
}
