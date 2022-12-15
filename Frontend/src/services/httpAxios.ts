import axios from "axios";

//on startup
export let customAxios = axios.create({
  baseURL: 'https://localhost:7265/',
  headers: {
    Authorization: `Bearer ${localStorage.getItem('token')}`
  }
})

//on login
export function reloadAxios() {
  customAxios = axios.create({
    baseURL: 'https://localhost:7265/',
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token')}`
    }
  })
}
