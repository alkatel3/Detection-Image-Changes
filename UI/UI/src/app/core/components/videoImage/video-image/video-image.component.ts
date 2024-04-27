import { Component, OnInit } from '@angular/core';
// import { ToggleService } from '../../../services/toggleService/toggle-service.service';

import { CommonModule } from '@angular/common';
import { ButtonToggleComponent } from "../../buttonToggle/button-toggle/button-toggle.component";
import { ApiService } from '../../../services/ApiService/api-service.service';
import { Camera } from '../../../model/camera';
@Component({
    selector: 'app-video-image',
    standalone: true,
    templateUrl: './video-image.component.html',
    styleUrl: './video-image.component.css',
    imports: [CommonModule, ButtonToggleComponent]
})
export class VideoImageComponent 
{
  videos : Camera[] =[]
  constructor(private apiService: ApiService) {
   }
  ngOnInit() {
    var temp = this.apiService.getCameras().subscribe((data: Camera[]) => {
      this.videos = data
    });
  }
dispayStyle: any;
changeDispllayStyleEvent($event: string) {
   this.dispayStyle = $event
}
DeleteVideo() {
throw new Error('Method not implemented.');
}
PlayOrPauseVideo(id: any) {
  var myVideo = document.getElementById(id) as HTMLVideoElement; 
  if (myVideo.paused) 
    myVideo.play(); 
  else 
    myVideo.pause();
 }
AddVideo() {
throw new Error('Method not implemented.');
}
  // videos = [
  //   { id: "Video 1", src: "../../../../../assets/images/mov_bbb.mp4" },
  //   { id: "Video 2", src: "../../../../../assets/images/mov_bbb.mp4" },
  //   { id: "Video 3", src: "../../../../../assets/images/mov_bbb.mp4" }
  // ];
}
