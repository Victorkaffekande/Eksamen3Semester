import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {NgbDate, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from "@angular/forms";
import {UserDto} from "../../../interfaces/userDto";
import {RegisterComponent} from "../../auth/register/register.component";
import {LoginService} from "../../../services/login.service";


@Component({
  selector: 'app-admin-register',
  templateUrl: './admin-register.component.html',
  styleUrls: ['./admin-register.component.sass']
})
export class AdminRegisterComponent implements OnInit {

  registerForm: FormGroup = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
    passwordConfirm: ['', Validators.required],
    email: ['', Validators.required],
    birthdate: ['', Validators.required],
    role: ['admin', Validators.required]
  }, {
    updateOn: 'submit',
    validators: RegisterComponent.passwordValidator('password', 'passwordConfirm')
  });
  accountCreated: any;
  errorMsg: any;
  @Output() notify = new EventEmitter<any>();

  constructor(private modalService: NgbModal,
              private fb: FormBuilder,
              private loginService: LoginService) {
  }

  ngOnInit(): void {

  }


  openModal(myModal: any) {
    this.modalService.open(myModal)
  }

  async register() {
    this.registerForm.markAsPristine();
    if (this.registerForm.valid) {

      var date = this.registerForm.get("birthdate")?.value as NgbDate;

      let dateString = RegisterComponent.formatDate(date)


      const dto: UserDto = {
        username: this.registerForm.get("username")?.value,
        password: this.registerForm.get("passwordConfirm")?.value,
        email: this.registerForm.get("email")?.value,
        birthday: dateString,
        role: this.registerForm.get('role')?.value
      }

      await this.loginService.register(dto)
        .then(u => {
            this.accountCreated = true
            this.notify.emit(u);
          }
        )
        .catch(error => {
          this.accountCreated = false;
          this.errorMsg = error.response.data;
        });
    }
  }

}
