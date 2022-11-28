import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ProjectDto} from "../../../interfaces/projectDto";
import {Token} from "../../../interfaces/token";
import jwtDecode from "jwt-decode";
import {ProjectService} from "../../../services/project.service";

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.sass']
})
export class CreateProjectComponent implements OnInit {

  createForm: FormGroup = this.fb.group(
    {
      image: [""],
      title: ["", Validators.required]
    },
  )
  selectedImage: any;

  constructor(private fb: FormBuilder,
              private service: ProjectService) {
  }

  ngOnInit(): void {
  }

  createProject() {
    if (!this.createForm.valid) return "createForm is not valid";

    let token = localStorage.getItem("token");
    if (!token) return "no token in local storage"

    let deToken = jwtDecode(token) as Token;

    let imageString = this.createForm.get("image")?.value
    if (imageString == "") imageString = undefined;

    const dto: ProjectDto = {
      UserId: deToken.userId, //get fra token
      PatternId: undefined, //null ?? måske lav et link til all patterns
      Image: imageString, //if tom input => null | fix
      Title: this.createForm.get("title")?.value,
      StartTime: new Date(Date.now()).toJSON(), //tjek om rigtigt, måske fucked
      IsActive: true
    }
    this.service.createProject(dto).then(() => console.log("project created"));
    return "project DTO created"
  }


  onFileSelectedPdf($event: Event) {
    const reader: FileReader = new FileReader();
    reader.onloadend = (e) => {
      this.selectedImage = reader.result;
      this.createForm.get("image")?.setValue(reader.result)
    }
    // @ts-ignore
    const file: File = event.target.files[0];
    if (file) {
      reader.readAsDataURL(file)
    }
  }
}
