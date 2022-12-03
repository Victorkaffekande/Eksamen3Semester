import {Injectable} from '@angular/core';
import {customAxios} from "./httpAxios";

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor() {
  }

  async createPost(dto: any) {
    const httpResult = await customAxios.post("Post/CreatePost", dto);
    return httpResult.data;
  }

  async getPostsFromProject(projectId: any): Promise<any> {
    const result = await customAxios.get("Post/GetAllPostByProject/" + projectId)
    return result.data;
  }
}
