import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { TemplateSelectorComponent } from './template-selector.component';

@Component({
  selector: 'app-template-selector-widget',
  templateUrl: './template-selector.component.html',
  styleUrls: ['./template-selector.component.scss'],
})
export class TemplateSelectorWidgetComponent extends TemplateSelectorComponent {
  loadTemplates(): void {
    this.isLoading = false;
  }
}
