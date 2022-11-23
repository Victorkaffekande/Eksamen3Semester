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

      type date = {
        year: string,
        month: string,
        day: string
      }
      var x = this.registerForm.get("birthdate")?.value as date;
      var w = x.year + "-" + x.month + "-" + x.day


      const dto: UserDto = {
        username: this.registerForm.get("username")?.value,
        password: this.registerForm.get("passwordConfirm")?.value,
        email: this.registerForm.get("email")?.value,
        birthday: w.toString(),
        role: "fillrole"
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
}
