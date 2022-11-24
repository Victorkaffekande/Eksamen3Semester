import { Injectable } from '@angular/core';
import {customAxios} from "./httpAxios";

@Injectable({
  providedIn: 'root'
})
export class PatternService {

  constructor() { }

  async getPatternsByUserId(id: any){
    const httpResult = await customAxios.get<any>("Pattern/GetAllPatternsByUser/" +id)
    console.log(httpResult.data);
    return httpResult.data;
  }

  async CreatePattern(dto: {Title : string; UserId : number; PdfString : string; Description : string; Image : string;}){
    const httpResult = await customAxios.post('Pattern/CreatePattern', dto)
    return httpResult.data;
  }
}
