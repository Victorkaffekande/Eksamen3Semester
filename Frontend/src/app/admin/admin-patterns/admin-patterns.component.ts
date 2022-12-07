import {Component, OnInit} from '@angular/core';
import {PatternService} from "../../../services/pattern.service";

@Component({
  selector: 'app-admin-patterns',
  templateUrl: './admin-patterns.component.html',
  styleUrls: ['./admin-patterns.component.sass']
})
export class AdminPatternsComponent implements OnInit {

  constructor(private patternService: PatternService) {
  }

  patternList: any;

  ngOnInit(): void {
    this.setPatternList();
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
}
