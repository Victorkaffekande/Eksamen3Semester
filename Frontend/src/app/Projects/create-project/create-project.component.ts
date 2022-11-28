import {Component, EventEmitter, OnInit, Output} from '@angular/core';
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
  @Output() notify = new EventEmitter<any>();

  constructor(private fb: FormBuilder,
              private service: ProjectService) {
  }

  ngOnInit(): void {
  }

  async createProject() {
    if (!this.createForm.valid) return "createForm is not valid";

    let token = localStorage.getItem("token");
    if (!token) return "no token in local storage"

    let deToken = jwtDecode(token) as Token;

    let imageString = this.createForm.get("image")?.value
    if (imageString == "") imageString = undefined;

    const dto: ProjectDto = {
      UserId: deToken.userId,
      PatternId: undefined, //null ?? måske lav et link til all patterns
      Image: imageString,
      Title: this.createForm.get("title")?.value,
      StartTime: new Date(Date.now()).toJSON(),
      IsActive: true
    }
    let a = await this.service.createProject(dto);
    this.emmitProject(a.data);
    return "project DTO created"
  }

  emmitProject(project: any){
    this.notify.emit(project);
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
