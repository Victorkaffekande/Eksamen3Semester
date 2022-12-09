import { Injectable } from '@angular/core';
import {customAxios} from "./httpAxios";

@Injectable({
  providedIn: 'root'
})
export class LikeService {

  constructor() { }


  async likeUser(dto: { userId:number, likedUserId:number}){
    const httpResult = await customAxios.post("Like/LikeUser" , dto )
  return httpResult.data
  }

  async removeLike(dto: {userId: number, likedUserId:number}){

    const httpResult = await customAxios.delete("Like/RemoveLike" , {data: dto})
    return httpResult.data
  }

  async alreadyLikes(dto: {userId: any, likedUserId:any}){
    const httpResult = await customAxios.post("Like/AlreadyLikes", dto)
    return httpResult.data
  }
  async getAllLikedUsersByUser(id:number){
    const httpResult = await customAxios.get("Like/GetLikedUsers/" + id )
    return httpResult.data
  }
  async getAllPostByLikedUsers(id:number, skip:number, take:number){
    const httpResult = await customAxios.get("Like/GetAllPostByLikedUsers/" +id+ "/" +skip+ "/" +take )
    return httpResult.data
  }
}
