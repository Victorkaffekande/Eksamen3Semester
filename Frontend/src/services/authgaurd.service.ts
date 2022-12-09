import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";
import jwtDecode from "jwt-decode";
import {Token} from "../interfaces/token";

@Injectable({
  providedIn: 'root'
})
export class AuthgaurdService {

  constructor() { }


  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    let token = localStorage.getItem('token')
    if (token) {
      let deToken = jwtDecode(token) as Token
      let currentDate = new Date();
      // @ts-ignore
      let expDate = new Date(deToken.exp * 1000)
      if (currentDate < expDate && deToken.role == "user" || "admin") {
        return true;
      }
    }
    return false;
  }


  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    let token = localStorage.getItem('token')
    if (token) {
      let deToken = jwtDecode(token) as Token
      let currentDate = new Date();
      // @ts-ignore
      let expDate = new Date(deToken.exp * 1000)
      if (currentDate < expDate && deToken.role == "admin") {
        return true;
      }
    }
    return false;
  }
}
