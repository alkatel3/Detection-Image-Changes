import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Camera } from '../../model/camera';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  getCameras() {
    return this.http.get<Camera[]>('https://localhost:7027/api/Camera');
  }
}
