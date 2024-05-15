import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Camera } from '../../model/camera';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  getCameras(): Observable<any> {
    return this.http.get<Camera>('https://localhost:7027/Camera/GetCameras');
  }  
  postCamera(): Observable<any> {
    return this.http.post<any>('https://localhost:7027/Camera/', {});
  }
  getStream(): Observable<string> {
    return new Observable(observer => {
      const eventSource = new EventSource('https://localhost:7027/Camera/');

      eventSource.onmessage = event => {
        observer.next(event.data);
      };

      eventSource.onerror = error => {
        observer.error('Stream error');
      };

      return () => {
        eventSource.close();
      };
    });
  }
}
