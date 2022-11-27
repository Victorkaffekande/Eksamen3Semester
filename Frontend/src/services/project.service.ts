import {Injectable} from '@angular/core';
import {ProjectDto} from "../interfaces/projectDto";
import {customAxios} from "./httpAxios";
import {Project} from "../interfaces/project";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor() {
  }

  async createProject(dto: ProjectDto): Promise<any> {
    return await customAxios.post("/createProject", dto);
  }

  async getAllProjectsFromUser(id: number): Promise<Project[]> {
    let response = await customAxios.get<Project[]>("GetProjectsFromUser/" + id)
    return response.data
  }

}
