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
}
