import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";

@Component({
  selector: 'app-my-patterns',
  templateUrl: './my-patterns.component.html',
  styleUrls: ['./my-patterns.component.sass']
})
export class MyPatternsComponent implements OnInit {
  firstFormGroup = this._formBuilder.group({
    firstCtrl: ['', Validators.required],
  });
  secondFormGroup = this._formBuilder.group({
    secondCtrl: ['', Validators.required],
  })

  constructor(private _formBuilder: FormBuilder) {
  }

  ngOnInit(): void {
  }


}
