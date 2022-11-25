import { Component, OnInit } from '@angular/core';
import {AbstractControl, FormBuilder, Validators} from "@angular/forms";
import jwtDecode from "jwt-decode";
import {Token} from "../../../interfaces/token";
import {PatternService} from "../../../services/pattern.service";

@Component({
  selector: 'app-create-pattern',
  templateUrl: './create-pattern.component.html',
  styleUrls: ['./create-pattern.component.sass']
})
export class CreatePatternComponent implements OnInit {
  image: any;
  pdf: any;
  id: number = 0;
  title: string =  "";
  description: string = "";

  constructor(private _formBuilder: FormBuilder, private patternService: PatternService) {
  }

  ngOnInit(): void {
    let t = localStorage.getItem("token");
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.id = deToken.userId;
    }

  }

  formGroup = this._formBuilder.group({
    title: ['', Validators.required],
    description: ['', Validators.required]
  });
  formGroupPdf = this._formBuilder.group({
    pdf: ['', Validators.required]
  });
  formGroupImage = this._formBuilder.group({
    image: ['', Validators.required]
  });


  //submit a pattern to database
  async submit() {
    let dto = {
      title: <string>this.formGroup.get('title')?.value,
      userId: 17,
      pdf: this.pdf,
      description: <string>this.formGroup.get("description")?.value,
      image: this.image
    }
    console.log(dto)

    const result = await this.patternService.CreatePattern(dto)
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
