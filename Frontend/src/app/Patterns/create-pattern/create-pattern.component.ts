import { Component, OnInit } from '@angular/core';
import {AbstractControl, FormBuilder, Validators} from "@angular/forms";
import jwtDecode from "jwt-decode";
import {Token} from "../../../interfaces/token";
import {PatternService} from "../../../services/pattern.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-create-pattern',
  templateUrl: './create-pattern.component.html',
  styleUrls: ['./create-pattern.component.sass']
})
export class CreatePatternComponent implements OnInit {
  //create pattern
  image: any;
  pdf: any;
  id: number = 0;
  title: string =  "";
  description: string = "";

  //confirms creation
  patternCreated = false;
  submitted = false;
  errorMsg: any;

  constructor(private _formBuilder: FormBuilder,
              private patternService: PatternService,
              private router: Router) {

  }

  formGroup = this._formBuilder.group({
    title: ['', Validators.required],
    difficulty : [''],
    description: ['', Validators.required]
  });

  formGroupInfo = this._formBuilder.group({
    yarn: [''],
    language : [''],
    needleSize: [''],
    gauge: ['']
  });
  formGroupPdf = this._formBuilder.group({
    pdf: ['', Validators.required]
  });
  formGroupImage = this._formBuilder.group({
    image: ['', Validators.required]
  });
  ngOnInit(): void {
    let t = localStorage.getItem("token");
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.id = deToken.userId;
    }

  }



  //submit a pattern to database
  async submit() {
    let dto = {
      title: <string>this.formGroup.get('title')?.value,
      userId: this.id,
      pdfstring: this.pdf,
      description: <string>this.formGroup.get("description")?.value,
      image: this.image,
      difficulty: <string>this.formGroup.get("difficulty")?.value,
      yarn: <string>this.formGroupInfo.get("yarn")?.value,
      language: <string>this.formGroupInfo.get("language")?.value,
      needleSize: <string>this.formGroupInfo.get("needleSize")?.value,
      gauge: <string>this.formGroupInfo.get("gauge")?.value
    }
    this.submitted = true

    await this.patternService.CreatePattern(dto)
      .then(() =>
      {
        this.patternCreated = true;
        this.router.navigate(["user/mypatterns"])
      }
      )
      .catch(error => {
        this.patternCreated = false;
        this.errorMsg = error.response.data;
      });
  }



  //convert selected image to a blob image
  onFileSelectedImage($event: Event) {
    // @ts-ignore
    const file: File = event.target.files[0];
    const reader: FileReader = new FileReader();
    reader.onloadend = (e) => {
      this.image = reader.result;
    }
    if (file) {
      reader.readAsDataURL(file)
    }
  }

  onFileSelectedPdf($event: Event) {
    // @ts-ignore
    const file: File = event.target.files[0];
    const reader: FileReader = new FileReader();
    reader.onloadend = (e) => {
      this.pdf = reader.result;
    }
    if (file) {
      reader.readAsDataURL(file)  // @ts-ignore
    }
  }
}
