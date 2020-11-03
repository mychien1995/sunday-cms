import { OnInit, Component } from '@angular/core';
import { AppHeaderComponent } from '@components/_layout';
import { LayoutModel } from '@core/models';
import { ColorService } from '@core/services';
import { LayoutService } from '@core/services';

@Component({
  selector: 'app-content-tree',
  templateUrl: './content-tree.component.html',
  styleUrls: ['./content-tree.component.scss'],
})
export class ContentTreeComponent implements OnInit {
  constructor() {
      console.log('loaded');
  }

  ngOnInit(): void {
    console.log('inited');
  }
}
