import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from "./core/components/navbar/navbar.component";
import { VideoImageComponent } from "./core/components/videoImage/video-image/video-image.component";
import { ApiService } from './core/services/ApiService/api-service.service';
import { HttpClientModule } from '@angular/common/http';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [RouterOutlet, NavbarComponent, VideoImageComponent, CommonModule, HttpClientModule ],
    providers:[ApiService]
})
export class AppComponent {
  title = 'UI';
}
