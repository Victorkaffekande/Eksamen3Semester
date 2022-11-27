import {Component, OnInit} from '@angular/core';
import {Token} from "../../../interfaces/token";
import jwtDecode from "jwt-decode";
import {ProjectService} from "../../../services/project.service";
import {ProjectCard} from "../../../interfaces/projectCard";
import {Project} from "../../../interfaces/project";


@Component({
  selector: 'app-my-projects',
  templateUrl: './my-projects.component.html',
  styleUrls: ['./my-projects.component.sass']
})
export class MyProjectsComponent implements OnInit {
  collapsed: boolean = true;

  constructor(private service: ProjectService) {
  }

  selected: any = "0";

  projectList: Project[] = [];

  ngOnInit(): void {
    this.getAllMyProjects();
  }

//get all projekts sort på active ud fra radio buttons
  select() {
    console.log(this.selected)
  }

   async getAllMyProjects() {
    let t = localStorage.getItem("token");
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.projectList =  await this.service.getAllProjectsFromUser(deToken.userId);
    }
  }
}
