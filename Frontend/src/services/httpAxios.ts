import axios from "axios";

export const customAxios = axios.create({
  baseURL: 'https://localhost:7265/',
  headers:{
    Authorization: `Bearer ${localStorage.getItem('token')}`
  }

})
