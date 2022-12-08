import {Component, OnInit} from '@angular/core';
import {LikeService} from "../../services/like.service";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {Router} from "@angular/router";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.sass']
})
export class DashboardComponent implements OnInit {
  postList: any;
  selectedPost: any;

  skip: number = 0;
  take: number = 10;
  batchSize: number = 10;

  constructor(private service: LikeService,
              private modalService: NgbModal,
              private router: Router) {
  }

  ngOnInit() {
    this.getInitialPosts();
  }

  async getInitialPosts() {
    this.postList = await this.service.getAllPostByLikedUsers(2, this.skip, this.take);
    this.skip += this.batchSize;
    this.take += this.batchSize;
  }

  async getMorePosts() {
    let a = await this.service.getAllPostByLikedUsers(2, 0, 10);
    this.postList.push(a);
  }

  openModal(modal: any, p: any) {
    this.selectedPost = p;
    this.modalService.open(modal, {size: 'xl'});
  }


  goToProject(id: any, modal: any) {
    this.router.navigate(['user/projectDetails/'+id])
    modal.dismiss();
  }
}
