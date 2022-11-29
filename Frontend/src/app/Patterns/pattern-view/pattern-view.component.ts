import {Component, Input, OnInit} from '@angular/core';
import {PatternService} from "../../../services/pattern.service";
import jwtDecode from "jwt-decode";
import {Token} from "../../../interfaces/token";
import {FormBuilder, Validators} from "@angular/forms";
import {DomSanitizer} from "@angular/platform-browser";

@Component({
  selector: 'app-pattern-view',
  templateUrl: './pattern-view.component.html',
  styleUrls: ['./pattern-view.component.sass']
})
export class PatternViewComponent implements OnInit {

  isButtonForEditDisabled: boolean = true;
  selectedPattern: any = undefined;

  id: number = 0;
  username: string = "default"

  constructor(private patternService: PatternService, private _formBuilder: FormBuilder, private sanitizer: DomSanitizer) {
  }

  ngOnInit(): void {
    this.selectedPattern = this.patternService.selectedPattern;

    let t = localStorage.getItem("token");
    if (t) {
      let deToken = jwtDecode(t) as Token;
      this.username = deToken.username;
      this.id = deToken.userId;
    }
    if (this.selectedPattern.userId == this.id) {
      this.isButtonForEditDisabled = false;
    }
    this.fillForm();
  }

  createPdfBtn(){
    let link = document.createElement('a');
    link.innerHTML = 'Download PDF file';
    link.download =  this.selectedPattern.title +'.pdf';
    link.href = this.selectedPattern.pdfString;
    link.click()

  }




  enableEditing() {
    this.formGroup.enable()
  }

  formGroup = this._formBuilder.group({
    title: [{value:''}],
    difficulty: [{value:''}],
    language: [{value:''}],
    needleSize: [{value:''}],
    gauge: [{value:''}],
    yarn: [{value:''}],
    description: [{value:''}],
    pdfString: [{value:''}],
    image: [{value:''}],
  });


  fillForm(){
    if (this.selectedPattern){
      this.formGroup.controls['title'].setValue(this.selectedPattern.title);
      this.formGroup.controls['difficulty'].setValue(this.selectedPattern.difficulty);
      this.formGroup.controls['language'].setValue(this.selectedPattern.language);
      this.formGroup.controls['needleSize'].setValue(this.selectedPattern.needleSize);
      this.formGroup.controls['gauge'].setValue(this.selectedPattern.gauge);
      this.formGroup.controls['yarn'].setValue(this.selectedPattern.yarn);
      this.formGroup.controls['description'].setValue(this.selectedPattern.description);
      this.formGroup.controls['pdfString'].setValue(this.selectedPattern.pdfString);
      this.formGroup.controls['image'].setValue(this.selectedPattern.image);
      this.formGroup.disable()
    }
  }


}
