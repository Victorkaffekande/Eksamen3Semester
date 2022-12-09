import { Component, OnInit } from '@angular/core';
import jwtDecode from "jwt-decode";
import { NgbCarouselModule } from '@ng-bootstrap/ng-bootstrap';
import {Token} from "../../../interfaces/token";
import {UserService} from "../../../services/user.service";
import {ActivatedRoute} from "@angular/router";
import {PatternService} from "../../../services/pattern.service";
import {ProjectService} from "../../../services/project.service";
import {LikeService} from "../../../services/like.service";
import {dispatchTouchEvent} from "@angular/cdk/testing/testbed/fake-events";

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
  projects: any;
  alreadyLikes: any = undefined;
  private errorMsg: any;

  constructor(private likeService: LikeService, private projectService: ProjectService,private patternService: PatternService,private userService: UserService, private activatedRoute: ActivatedRoute) { }


  async ngOnInit(){
    let t = localStorage.getItem("token");
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.userId = deToken.userId;
    }
    this.userRouteId = this.activatedRoute.snapshot.paramMap.get('id');


    this.user = await this.userService.getUserById(this.userRouteId)

    this.patterns = await this.patternService.getPatternsByUserId(this.userRouteId)
    this.projects = await this.projectService.getAllProjectsFromUser(this.userRouteId)
    let dto = {
      userId: this.userId,
      likedUserId: this.userRouteId
    }
    if (this.userId != this.userRouteId){
    await this.likeService.alreadyLikes(dto).then(r => this.alreadyLikes = r)
    }
  }

  changeSubView() {
    this.showEditScreen = true;
  }

  async likeUser() {
    let dto = {
      userId: this.userId,
      likedUserId: this.userRouteId
    }

    this.likeService.likeUser(dto).then( () =>
      this.alreadyLikes = true
    )
      .catch(error => {
        this.errorMsg = error.response.data;
      });
  }

  async deleteLike() {
    let dto = {
      userId: this.userId,
      likedUserId: this.userRouteId
    }

    this.likeService.removeLike(dto).then( () =>
      this.alreadyLikes = false
    )
      .catch(error => {
        this.errorMsg = error.response.data;
      });
  }
}
