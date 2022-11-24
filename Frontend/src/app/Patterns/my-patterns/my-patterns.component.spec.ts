import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyPatternsComponent } from './my-patterns.component';

describe('MyPatternsComponent', () => {
  let component: MyPatternsComponent;
  let fixture: ComponentFixture<MyPatternsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyPatternsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyPatternsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
