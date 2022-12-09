import {Component, OnInit} from '@angular/core';
import {UserService} from "../../../services/user.service";
import jwtDecode from "jwt-decode";
import {Token} from "../../../interfaces/token";

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.sass']
})
export class AdminUsersComponent implements OnInit {

  constructor(private service: UserService) {
  }

  userList: any = [];
  adminList: any = [];
  adminId: any;

  ngOnInit(): void {
    this.getAdminId()
    this.getAllUsers();
    this.getAllAdmins()
  }

  getAdminId() {
    let t = localStorage.getItem('token');
    if (t) {
      let deT = jwtDecode(t) as Token;
      this.adminId = deT.userId;
    }
  }

  async getAllUsers() {
    this.userList = await this.service.getAllUsers();
  }

  async getAllAdmins() {
    this.adminList = await this.service.getAllAdmins();
  }

  async deleteAdmin(id: any) {
    if (id == this.adminId) {
      confirm("Du kan ikke slette dig selv")
      return
    }
    if (confirm("Er du sikker på at du vil slette?")) {
      let r = await this.delete(id);
      this.adminList = this.adminList.filter((u: { id: any }) => u.id != r.id)
    }
  }

  async deleteUser(id: any) {
    if (confirm("Er du sikker på at du vil slette?")) {
      let r = await this.delete(id);
      this.userList = this.userList.filter((u: { id: any }) => u.id != r.id)
    }
  }

  async delete(id: any) {
    return await this.service.deleteUser(id);
  }

  addNewUser(user: any) {
    location.reload();
  }
}
