import {Component, OnInit} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators
} from "@angular/forms";
import {LoginService} from "../../../services/login.service";
import {UserDto} from "../../../interfaces/userDto";
import {stringifyTask} from "@angular/compiler-cli/ngcc/src/execution/tasks/utils";
import {NgbDate} from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.sass']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
    passwordConfirm: ['', Validators.required],
    email: ['', Validators.required],
    birthdate: ['', Validators.required]
  }, {
    updateOn: 'submit',
    validators: RegisterComponent.passwordValidator('password', 'passwordConfirm')
  });

  accountCreated = false;
  errorMsg: any;
  test: any;

  constructor(private fb: FormBuilder,
              private loginService: LoginService) {

  }

  ngOnInit(): void {
  }

  static passwordValidator(fieldA: string, fieldB: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {

      const formGroup = control as FormGroup;
      const valueA = formGroup.get(fieldA)?.value;
      const valueB = formGroup.get(fieldB)?.value;

      if (valueA === valueB) {
        return null;
      } else {
        return {valuesDoNotMatch: true}
      }

    }
  }

  async register() {
    this.registerForm.markAsPristine();
    if (this.registerForm.valid) {

      var date = this.registerForm.get("birthdate")?.value as NgbDate;

      let dateString = RegisterComponent.formatDate(date);

      const dto: UserDto = {
        username: this.registerForm.get("username")?.value,
        password: this.registerForm.get("passwordConfirm")?.value,
        email: this.registerForm.get("email")?.value,
        birthday: dateString,
        role: "user"
      }

      await this.loginService.register(dto)
        .then(() =>
          this.accountCreated = true
        )
        .catch(error => {
          this.accountCreated = false;
          this.errorMsg = error.response.data;
        });
    }
  }

 static formatDate(d: NgbDate): string {
    let day = String(d.day);
    if (d.day < 10) {
      day = '0' + day
    }

    let month = String(d.month);
    if (d.month < 10) {
      month = '0' + month
    }
    return d.year + "-" + month + "-" + day;

  }
}
