import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {ProjectService} from "../../../services/project.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {NgbCollapse} from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.sass']
})
export class ProjectDetailsComponent implements OnInit {

  project: any;
  projectId: any;
  postList: any;
  editCollapsed: boolean = true;
  oldImage: any;

  updateForm: FormGroup = this.fb.group({
    title: ['', Validators.required],
    image: ['']
  })

  constructor(private activatedRoute: ActivatedRoute,
              private service: ProjectService,
              private router: Router,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.projectId = this.activatedRoute.snapshot.params['id'];
    this.getProject()
      .then(() => this.fillForm());


  }

  async getProject() {
    this.project = await this.service.getSingleProjectFromId(this.projectId);
  }

  fillForm() {
    if (this.project) {
      this.updateForm.get('title')?.setValue(this.project.title);
      this.oldImage = this.project.image;
    }
  }

  async deleteProject() {
    let msg: string = "Er du sikker på at du vil slette " + this.project.title
    if (confirm(msg)) {
      let result = await this.service.deleteProject(this.project.id)
        .then(() => this.router.navigate(['user/myprojects']));
    }
  }


  updateProject() {
    if (this.updateForm.valid) {
      this.project.title = this.updateForm.get('title')?.value;
      let x = this.service.updateProject(this.project);
      console.log(x);
    }
  }

  onFileSelectedPdf($event: Event) {
    const reader: FileReader = new FileReader();
    reader.onloadend = (e) => {
      this.project.image = reader.result;
      this.updateForm.get("image")?.setValue(reader.result)
    }
    // @ts-ignore
    const file: File = event.target.files[0];
    if (file) {
      reader.readAsDataURL(file)
    }
  }

  cancelUpdate(collapse: NgbCollapse) {
    this.project.image = this.oldImage;
    collapse.toggle();
  }

  async endProject() {
    this.project.isActive = false;
    let response = await this.service.updateProject(this.project);
  }

  async restartProject() {
    this.project.isActive = true;
    let response = await this.service.updateProject(this.project);
  }
}
