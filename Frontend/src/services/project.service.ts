import {Injectable} from '@angular/core';
import {ProjectDto} from "../interfaces/projectDto";
import {customAxios} from "./httpAxios";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor() {
  }

  async createProject(dto: ProjectDto): Promise<any> {
    return await customAxios.post("Project/createProject", dto);
  }

  async getAllProjectsFromUser(id: number): Promise<any> {
    let response = await customAxios.get<any>("Project/GetProjectsFromUser/" + id);
    return response.data
  }

  async updateProject(project: any){
    let response = await customAxios.put<any>("Project/UpdateProject",project);
    return response.data
  }

}
