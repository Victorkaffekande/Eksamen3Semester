import {Component, Input, OnInit} from '@angular/core';
import {PatternService} from "../../../services/pattern.service";
import jwtDecode from "jwt-decode";
import {Token} from "../../../interfaces/token";
import {FormBuilder, Validators} from "@angular/forms";
import {DomSanitizer} from "@angular/platform-browser";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-pattern-view',
  templateUrl: './pattern-view.component.html',
  styleUrls: ['./pattern-view.component.sass']
})
export class PatternViewComponent implements OnInit {

  isButtonForEditDisabled: boolean = true;
  doesPatternExist: boolean = false;
  selectedPattern: any = undefined;

  id: number = 0;
  username: string = "default"

  patternId: any;
  errorMsg: any;

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private patternService: PatternService, private _formBuilder: FormBuilder) {
  }

  async ngOnInit() {

    this.patternId = this.activatedRoute.snapshot.paramMap.get('id');

    const pattern = await this.patternService.getPatternById(this.patternId);
    this.selectedPattern = pattern;

    if (this.selectedPattern.title != null) {
      this.doesPatternExist = true;
    }

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

  createPdfBtn() {
    let link = document.createElement('a');
    link.innerHTML = 'Download PDF file';
    link.download = this.selectedPattern.title + '.pdf';
    link.href = this.selectedPattern.pdfString;
    link.click()

  }


  enableEditing() {
    this.formGroup.enable()
  }

  formGroup = this._formBuilder.group({
    title: [{value: ''}],
    difficulty: [{value: ''}],
    language: [{value: ''}],
    needleSize: [{value: ''}],
    gauge: [{value: ''}],
    yarn: [{value: ''}],
    description: [{value: ''}],
    pdfString: [{value: ''}],
    image: [{value: ''}],
  });


  fillForm() {
    if (this.selectedPattern) {
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


  async deletePattern() {

    if (confirm("Opskriften ville blive fjernet for evigt")) {
      await this.patternService.deletePattern(this.patternId)
        .then(async () =>
          await this.router.navigate(["/user/mypatterns"])
        )
        .catch(error => {
          this.errorMsg = error.response.data;
        });
    }


  }

  async submitEdit() {
    let dto = {
      title: <string><unknown>this.formGroup.get('title')?.value,
      id: this.patternId,
      userId: this.selectedPattern.userId,
      difficulty: <string><unknown>this.formGroup.get('difficulty')?.value,
      language: <string><unknown>this.formGroup.get('language')?.value,
      needleSize: <string><unknown>this.formGroup.get('needleSize')?.value,
      gauge: <string><unknown>this.formGroup.get('gauge')?.value,
      yarn: <string><unknown>this.formGroup.get('yarn')?.value,
      description: <string><unknown>this.formGroup.get('description')?.value,
      image: this.selectedPattern.image,
      PdfString: this.selectedPattern.pdfString,
    }
    console.log(this.selectedPattern.pdf)

    await this.patternService.updatePattern(dto)
      .then(() =>
        window.location.reload()
      )
      .catch(error => {
        this.errorMsg = error.response.data;
      });

  }

  //convert selected image to a blob image
  onFileSelectedImage($event: Event) {
    // @ts-ignore
    const file: File = event.target.files[0];
    const reader: FileReader = new FileReader();
    reader.onloadend = (e) => {
      this.selectedPattern.image = reader.result;
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
      this.selectedPattern.pdfString = reader.result;
    }
    if (file) {
      reader.readAsDataURL(file)  // @ts-ignore
    }
  }


  cancelEdit() {
    window.location.reload()
  }
}
