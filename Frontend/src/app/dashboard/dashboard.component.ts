import {Component, OnInit} from '@angular/core';
import {LikeService} from "../../services/like.service";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {Router} from "@angular/router";
import jwtDecode from "jwt-decode";
import {Token} from "../../interfaces/token";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.sass']
})
export class DashboardComponent implements OnInit {
  postList: any;
  selectedPost: any;

  skip: number = 0;
  take: number = 3;
  userId: any;

  constructor(private service: LikeService,
              private modalService: NgbModal,
              private router: Router) {
  }

  ngOnInit() {
    this.setUserId();
    this.getMorePosts();
  }

  setUserId() {
    let t = localStorage.getItem('token');
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.userId = deToken.userId;
    }
  }

  async getMorePosts() {

    let r = await this.service.getAllPostByLikedUsers(this.userId, this.skip, this.take);
    this.skip += this.take;

    if (this.postList == undefined)
      this.postList = r;
    else
      this.postList.push.apply(this.postList, r);
  }

  openModal(modal: any, p: any) {
    this.selectedPost = p;
    this.modalService.open(modal, {size: 'xl'});
  }


  goToProject(id: any, modal: any) {
    this.router.navigate(['user/projectDetails/' + id])
    modal.dismiss();
  }

  goToProfile(userId: any) {
    this.router.navigate(['user/userprofile/' + userId])
  }
}
