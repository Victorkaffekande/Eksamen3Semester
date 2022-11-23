import { Injectable } from '@angular/core';
import {UserDto} from "../interfaces/userDto";
import {customAxios} from "./httpAxios";

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor() { }

  async login(userDto: UserDto): Promise<any> {
    const httpResult = await customAxios.post('Auth/login', userDto);
    return httpResult.data;
  }

  async register(dto: UserDto): Promise<any> {
    const httpResult = await customAxios.post('Auth/register', dto);
    return httpResult.data;
  }

}
