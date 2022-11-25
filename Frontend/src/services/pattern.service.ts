import { Injectable } from '@angular/core';
import {customAxios} from "./httpAxios";
import {MatSnackBar} from "@angular/material/snack-bar";
import {catchError} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class PatternService {

  constructor() {

  }

  async getPatternsByUserId(id: any){
    const httpResult = await customAxios.get<any>("Pattern/GetAllPatternsByUser/" +id);
    console.log(httpResult.data);
    return httpResult.data;
  }

  async CreatePattern(dto: { title: string; userId: number;  pdf: string ; description: string; image: string}){
    let httpResult = await customAxios.post("/Pattern/CreatePattern", dto)
    return httpResult.data;

  }
}
