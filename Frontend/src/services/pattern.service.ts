import { Injectable } from '@angular/core';
import {customAxios} from "./httpAxios";
import {MatSnackBar} from "@angular/material/snack-bar";
import {catchError} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class PatternService {

  public selectedPattern: any;
  constructor() {

  }

  async getPatternsByUserId(id: any){
    const httpResult = await customAxios.get<any>("Pattern/GetAllPatternsByUser/" +id);
    return httpResult.data;
  }

  async CreatePattern(dto: { title: string; userId: number;  pdfstring: string ; description: string; image: string; difficulty:string; yarn:string; language:string; needleSize:string; gauge:string;}){
    let httpResult = await customAxios.post("pattern/createpattern", dto)
    return httpResult.data;

  }
}
