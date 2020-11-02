import { OnInit, Component } from '@angular/core';
import { AppHeaderComponent } from '@components/_layout';
import { LayoutModel } from '@core/models';
import { ColorService } from '@core/services';
import { LayoutService } from '@core/services';

@Component({
  selector: 'app-content-dashboard',
  templateUrl: './content-dashboard.component.html',
})
export class ContentDashboardComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}
}
