import { Injectable } from '@angular/core';
import {customAxios} from "./httpAxios";
import * as http from "http";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor() { }

  async getUserById(id: any) {
    const httpResult = await customAxios.get<any>("User/GetUserById/" + id)
    return httpResult.data
  }
  async updateUser(user: any) {
    let response = await customAxios.put<any>("User/UpdateUser", user);
    return response.data
  }
  async getAllUsers(){
    const httpResult = await customAxios.get("User/GetAllUsers")
    return httpResult.data
  }
  async getAllAdmins(){
    const httpResult = await customAxios.get("User/GetAllAdmins")
    return httpResult.data
  }

  async deleteUser(id:any){
    const httpResult = await customAxios.delete("User/DeleteUser/" + id);
    return httpResult.data;
  }
}
