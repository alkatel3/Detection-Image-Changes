import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetectedChangesComponent } from './detected-changes.component';

describe('DetectedChangesComponent', () => {
  let component: DetectedChangesComponent;
  let fixture: ComponentFixture<DetectedChangesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetectedChangesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DetectedChangesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
