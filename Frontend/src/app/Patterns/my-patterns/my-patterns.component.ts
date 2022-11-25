
import { Component, OnInit } from '@angular/core';
import jwtDecode from "jwt-decode";
import {Token} from "../../../interfaces/token";
import {Router} from "@angular/router";
import {PatternService} from "../../../services/pattern.service";

@Component({
  selector: 'app-my-patterns',
  templateUrl: './my-patterns.component.html',
  styleUrls: ['./my-patterns.component.sass']
})
export class MyPatternsComponent implements OnInit {

  constructor(private router: Router, private patternService: PatternService) { }
  id: number = 0;
  username: string = "default"

  patterns : any;

  async ngOnInit() {
    let t = localStorage.getItem("token");
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.username = deToken.username;
      this.id = deToken.userId;
    }
    const patterns = await this.patternService.getPatternsByUserId(this.id);
    this.patterns = patterns;


  }


}
