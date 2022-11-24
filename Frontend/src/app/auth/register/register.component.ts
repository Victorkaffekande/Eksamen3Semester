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
import {formatDate} from "@angular/common";

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
    validators: this.passwordValidator('password', 'passwordConfirm')
  });

  accountCreated = false;
  errorMsg: any;
  test: any;

  constructor(private fb: FormBuilder,
              private loginService: LoginService) {

  }

  ngOnInit(): void {
  }

  private passwordValidator(fieldA: string, fieldB: string): ValidatorFn {
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

      var x = this.registerForm.get("birthdate")?.value as NgbDate;

      let day = String(x.day);
      if (x.day < 10) {
        day = '0' + day
      }

      let month = String(x.month);
      if (x.month < 10) {
        month = '0' + month
      }

      let dateString = x.year + "-" + month + "-" + day;

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

  formatDate(d: NgbDate): string | null {
    if (d === null) {
      return null;
    }

    return [
      (d.day < 10 ? ('0' + d.day) : d.day),
      (d.month < 10 ? ('0' + d.month) : d.month),
      d.year
    ].join('-');
  }
}
