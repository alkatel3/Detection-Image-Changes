import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
// import { ToggleService } from '../../../services/toggleService/toggle-service.service';

import { CommonModule } from '@angular/common';
import { ButtonToggleComponent } from "../../buttonToggle/button-toggle/button-toggle.component";
import { ApiService } from '../../../services/ApiService/api-service.service';
import { Camera } from '../../../model/camera';
import { MAT_DIALOG_DATA, MatDialog, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Subscription } from 'rxjs';

export interface DialogData {
  video: string;
  name: string;
}
@Component({
    selector: 'app-video-image',
    standalone: true,
    templateUrl: './video-image.component.html',
    styleUrl: './video-image.component.css',
    imports: [CommonModule, ButtonToggleComponent]
})


export class VideoImageComponent 
{

  cameras : Camera[] =[]
  currentCamera: Camera | undefined;
  private streamSubscription: Subscription | undefined;
  showLoadingImage: boolean =false
  showLoadingCameraImage: boolean =false
  constructor(private apiService: ApiService, public dialog: MatDialog, private cdr: ChangeDetectorRef) {
   }
   ngOnInit(): void {
    this.getAccessCameras();
  }
  getAccessCameras() {
    this.showLoadingImage = true
    this.apiService.getCameras().subscribe(
      response=> {
      this.showLoadingImage = false
        this.cameras =response
      },
      error => {
        debugger
      this.showLoadingImage = false
        console.error(error);
      }
    )
  }

  ngOnDestroy(): void {
    if (this.streamSubscription) {
      this.streamSubscription.unsubscribe();
    }
  }

  streamVideo(port: number): void {
    const eventSource = new EventSource(`https://localhost:7027/Camera/StartCameraStream?port=${port}`);

    eventSource.onmessage = (event) => {
      
    if (this.currentCamera!==undefined) {
      this.currentCamera.src  = event.data
      this.showLoadingCameraImage = false
    }
      this.cdr.detectChanges();
    };
    var button = document.getElementById('stopButton-'+ port)
    button?.addEventListener('click', closeEventSource);
    function closeEventSource(){
            eventSource.close();
            button?.removeEventListener('click', closeEventSource)

    }
  }
dispayStyle: any;
changeDispllayStyleEvent($event: string) {
   this.dispayStyle = $event
}
PlayVideo(camera: Camera) {
  if(this.currentCamera){
    var stopButton = document.getElementById('stopButton-'+this.currentCamera.port);
    if(stopButton){
      stopButton?.click();
    }
  }
  this.currentCamera = camera
  this.showLoadingCameraImage = true
  this.cdr.detectChanges();
  this.streamVideo(camera.port)
 }
 stopStream() {
  this.currentCamera =undefined
  this.cdr.detectChanges();
}
}


@Component({
  selector: 'add-video',
  templateUrl: './add-video.html',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
  ],
})
export class addVideo {

  constructor(
    public dialogRef: MatDialogRef<VideoImageComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
  
  onFileSelected($event: Event) {
      debugger
      var input = $event?.target as HTMLInputElement;
      if (input.files && input.files.length > 0) {
        const video = input.files[0];
      }
    }
}
