import { Injectable } from '@angular/core';
import {customAxios} from "./httpAxios";

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
}
