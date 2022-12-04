import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {ProjectService} from "../../../services/project.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {NgbCarousel, NgbCollapse, NgbModal, NgbSlide, NgbSlideEvent} from "@ng-bootstrap/ng-bootstrap";
import {PostService} from "../../../services/post.service";

@Component({
  selector: 'app-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.sass']
})
export class ProjectDetailsComponent implements OnInit {


  project: any;
  projectId: any;
  postList: any;
  carouselList: any;
  editCollapsed: boolean = true;
  oldImage: any;



  updateForm: FormGroup = this.fb.group({
    title: ['', Validators.required],
    image: ['']
  })


  constructor(private activatedRoute: ActivatedRoute,
              private projectService: ProjectService,
              private postService: PostService,
              private router: Router,
              private fb: FormBuilder,
              private modalService: NgbModal) {
  }

  ngOnInit(): void {
    this.projectId = this.activatedRoute.snapshot.params['id']; // get id from browser route
    this.getProject()
      .then(() => this.fillForm());

    this.fillPostList();

  }

  /**
   * Gets one project from rest api
   */
  async getProject() {
    this.project = await this.projectService.getSingleProjectFromId(this.projectId);
  }

  //fills out the project Form group
  fillForm() {
    if (this.project) {
      this.updateForm.get('title')?.setValue(this.project.title);
      this.oldImage = this.project.image;
    }
  }

  async fillPostList() {
    this.postList = await this.postService.getPostsFromProject(this.projectId);
  }

  async deleteProject() {
    let msg: string = "Er du sikker på at du vil slette " + this.project.title
    if (confirm(msg)) {
      let result = await this.projectService.deleteProject(this.project.id)
        .then(() => this.router.navigate(['user/myprojects']));
    }
  }

  updateProject(collapse: NgbCollapse) {
    if (this.updateForm.valid) {
      this.project.title = this.updateForm.get('title')?.value;
      let x = this.projectService.updateProject(this.project);
      console.log(x);
      collapse.toggle();
    }
  }

  /**
   * triggered on a change in the file picker
   * converts the picked file into a base64 string
   * @param $event the new file
   */
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

  // returns the project image the oldImage
  cancelUpdate(collapse: NgbCollapse) {
    this.project.image = this.oldImage;
    collapse.toggle();
  }

  /**
   * updates the project to no longer be an active project
   */
  async endProject() {
    this.project.isActive = false;
    let response = await this.projectService.updateProject(this.project);
  }

  /**
   * updates the project to be active again
   */
  async restartProject() {
    this.project.isActive = true;
    let response = await this.projectService.updateProject(this.project);
  }

  /**
   * adds a new post to the postlist in frontend only
   * @param post a post
   */
  pushNewPost(post: any) {
    console.log("push log:")
    console.log(post)
    this.postList.push(post);
  }

  @ViewChild('carousel') carousel: any;


  /**
   * opens the modal (pop out window ontop of browserwindow)
   */
  async openModal(modal: any, c: NgbCarousel, clickedPost: any) {
    this.carouselList = this.modifyList(this.postList.slice(), clickedPost)
    await this.modalService.open(modal, {size: 'xl'});
  }

  /**
   * Reorganize the list so the place in the list where you want to start viewing in the beginning item of the list.
   * because the carousel is starting from the beginning of a list no matter what
   * @param list the list of post your viewing
   * @param clickedPost the post from where you want to start viewing
   */
  modifyList(list: any, clickedPost: any): Promise<any> {
    let cIndex = list.indexOf(clickedPost);
    let a = list.slice(0, cIndex)
    let b = list.slice(cIndex, list.length + 1)
    return b.concat(a)
  }

  navigateToRecipie(patternId: any) {
    this.router.navigate(['user/viewpattern',patternId]);
  }
}
