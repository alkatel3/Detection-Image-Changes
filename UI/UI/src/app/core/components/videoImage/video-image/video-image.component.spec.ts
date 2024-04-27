import { ComponentFixture, TestBed } from '@angular/core/testing';
import { VideoImageComponent } from './video-image.component';

describe('VideoImageComponent', () => {
  let component: VideoImageComponent;
  let fixture: ComponentFixture<VideoImageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VideoImageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VideoImageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
