import { Component } from '@angular/core';
import { ApiService } from '../../../services/ApiService/api-service.service';

@Component({
  selector: 'app-detected-changes',
  standalone: true,
  imports: [],
  templateUrl: './detected-changes.component.html',
  styleUrl: './detected-changes.component.css'
})
export class DetectedChangesComponent {


images: any;
showLoadingImage: any;
hidenImages: boolean =false;
ShowHideButtonText: string='Показати виявлені зміни';
constructor(private apiService: ApiService) {
}
ngOnInit(): void {
 this.getDetectedChanges();
}
getDetectedChanges() {
 this.showLoadingImage = true
 this.apiService.getDetectedChanges(0).subscribe(
   response=> {
   this.showLoadingImage = false
     this.images =response
   },
   error => {
     debugger
   this.showLoadingImage = false
     console.error(error);
   }
 )
}
showImages() {
  this.Reload()
  this.hidenImages = !this.hidenImages
  if(this.hidenImages){
    this.ShowHideButtonText='Сховати виявлені зміни';
  } else{
    this.ShowHideButtonText='Показати виявлені зміни';
  }
}
Reload() {
  this.getDetectedChanges();
}
Clear() {
  this.showLoadingImage = true
  this.apiService.ClearImageHistory().subscribe(
    response=> {
    this.showLoadingImage = false
      this.images =[]
    },
    error => {
      debugger
    this.showLoadingImage = false
      console.error(error);
    }
  )
}
}
