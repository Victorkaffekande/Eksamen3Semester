import { Component, OnInit } from '@angular/core';
import {LikeService} from "../../services/like.service";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.sass']
})
export class DashboardComponent implements OnInit {
  postList: any;

  constructor(private service: LikeService) { }

  ngOnInit(): void {
this.getPosts()
  }

  async getPosts(){
    this.postList = await this.service.getAllPostByLikedUsers(3,0,10);
  }

}
