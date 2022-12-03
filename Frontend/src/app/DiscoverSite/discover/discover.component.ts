import { Component, OnInit } from '@angular/core';
import {PatternService} from "../../../services/pattern.service";

@Component({
  selector: 'app-discover',
  templateUrl: './discover.component.html',
  styleUrls: ['./discover.component.sass']
})
export class DiscoverComponent implements OnInit {

  allPatterns :any;
  searchText: any;
  constructor(private patternService: PatternService) { }

  async ngOnInit(){
    const result = await this.patternService.getAllPattern();
    this.allPatterns = result;
  }

}
