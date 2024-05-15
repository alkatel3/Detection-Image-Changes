import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogOverviewExampleDialog } from './modal-window.component';

describe('ModalWindowComponent', () => {
  let component: DialogOverviewExampleDialog;
  let fixture: ComponentFixture<DialogOverviewExampleDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DialogOverviewExampleDialog]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DialogOverviewExampleDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
