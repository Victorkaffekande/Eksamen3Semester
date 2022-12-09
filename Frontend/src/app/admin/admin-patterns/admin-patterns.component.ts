import {Component, OnInit} from '@angular/core';
import {PatternService} from "../../../services/pattern.service";
import {ProjectService} from "../../../services/project.service";

@Component({
  selector: 'app-admin-patterns',
  templateUrl: './admin-patterns.component.html',
  styleUrls: ['./admin-patterns.component.sass']
})
export class AdminPatternsComponent implements OnInit {

  constructor(private patternService: PatternService,
              private projectService: ProjectService) {
  }

  projectList: any;
  patternList: any;

  ngOnInit(): void {
    this.setProjectList();
    this.setPatternList();
  }

  async setProjectList() {
    this.projectList = await this.projectService.getAllProjects();
  }

  async setPatternList() {
    this.patternList = await this.patternService.getAllPattern();
  }

  async deletePattern(id: any) {
    if (confirm("Er du sikker på at du vil slette?")) {
      let result = await this.patternService.deletePattern(id);
      this.patternList = this.patternList.filter((p: { id: any }) => result.id != p.id);
    }
  }

  deleteProject(id:any) {
    
  }
}
