import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {PostService} from "../../../services/post.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ModalDismissReasons, NgbModal} from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.sass']
})
export class CreatePostComponent implements OnInit {

  selectedImage: any;

  @Input() projectId: any;
  @Output() notify = new EventEmitter<any>();

  createForm: FormGroup = this.fb.group(
    {
      description: ["", Validators.required],
      image: ["", Validators.required]
    },
  )

  constructor(private service: PostService,
              private fb: FormBuilder,
              private modalService: NgbModal) {
  }

  ngOnInit(): void {
  }

  open(content: any) {
    this.modalService.open(content);
  }

  submitForm(modal: any) {

    if (this.createForm.valid) {
      let dto = {
        projectId: this.projectId,
        description: this.createForm.get('description')?.value,
        postDate: new Date(Date.now()).toJSON(),
        image: this.createForm.get('image')?.value
      }
      this.service.createPost(dto).then(r => this.notify.emit(r));
      modal.dismiss();
      this.resetModalView()
    }
  }

  resetModalView() {
    this.createForm.reset();
    this.selectedImage = "";
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
