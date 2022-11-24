import {Component, OnInit} from '@angular/core';
import jwtDecode from "jwt-decode";
import {Token} from "../../interfaces/token";
import {debounceTime} from "rxjs";
import {Router} from "@angular/router";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.sass']
})
export class HeaderComponent implements OnInit {

  constructor(private router: Router) {

  }

  username: string = "default"

  ngOnInit(): void {
   let t =  localStorage.getItem("token");
   if (t){
     let deToken = jwtDecode(t) as Token;
     this.username = deToken.username;
   }
  }
}
