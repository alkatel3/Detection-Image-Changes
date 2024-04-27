import {Component, EventEmitter, Output} from '@angular/core';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonToggleModule} from '@angular/material/button-toggle';

/**
 * @title Exclusive selection
 */
@Component({
  selector: 'app-button-toggle',
  templateUrl: 'button-toggle.component.html',
  styleUrl: 'button-toggle.component.css',
  standalone: true,
  imports: [MatButtonToggleModule, MatIconModule],
})

export class ButtonToggleComponent {
  selectedAlignment = 'left';
  @Output() changeDispllayStyleEvent = new EventEmitter<string>();
  changeDispllayStyle(group: any) {
    this.selectedAlignment = group
    this.changeDispllayStyleEvent.emit(group);
    }
}
