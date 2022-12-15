import {Component, OnInit} from '@angular/core';
import {Token} from "../../../interfaces/token";
import jwtDecode from "jwt-decode";
import {ProjectService} from "../../../services/project.service";
import {Router} from "@angular/router";
import {PostService} from "../../../services/post.service";


@Component({
  selector: 'app-my-projects',
  templateUrl: './my-projects.component.html',
  styleUrls: ['./my-projects.component.sass']
})
export class MyProjectsComponent implements OnInit {


  constructor(private service: ProjectService,
              private router: Router) {
  }


  projectList: any = [];
  filteredList: any=[];
  showIsActive: number = 1;

  ngOnInit(): void {
    this.getAllMyProjects().then(() => this.filterList());
  }


  async getAllMyProjects() {
    let t = localStorage.getItem("token");
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.projectList = await this.service.getAllProjectsFromUser(deToken.userId);
    }
  }

  async filterList() {
    if (this.showIsActive == 1)
      this.filteredList = this.projectList.filter((p: { isActive: any }) => p.isActive == true)
    else
      this.filteredList = this.projectList.filter((p: { isActive: any }) => p.isActive == false)
  }

  pushNewProject(project: any) {
    console.log(project);
    this.projectList.push(project);
    this.filterList();
  }

  radioClicked(event: any) {
    this.showIsActive = event.value;
    this.filterList();
  }

  async endProject(project: any) {
    project.isActive = false;
    let response = await this.service.updateProject(project).then(() => this.filterList())
  }

  async restartProject(project: any) {
    project.isActive = true;
    let response = await this.service.updateProject(project).then(() => this.filterList())
  }

  goToProject(project: any) {
    this.router.navigate(['user/projectDetails',project.id]);

  }
}
