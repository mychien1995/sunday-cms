import { OnInit, Component, ElementRef, ViewChild } from '@angular/core';
import { AppHeaderComponent } from '@components/_layout';
import { LayoutModel } from '@core/models';
import { ColorService } from '@core/services';
import { LayoutService } from '@core/services';

@Component({
  selector: 'app-content-sidebar',
  templateUrl: './app-sidebar-content.component.html',
  styleUrls: ['./app-sidebar-content.component.scss'],
})
export class AppContentSidebarComponent implements OnInit {
  layoutModel: LayoutModel;
  bgClass = '';
  constructor(
    public layoutService: LayoutService,
    public colorService: ColorService
  ) {
    this.setColor();
    layoutService.layoutBus.subscribe((data) => {
      if (data.event === 'layout-updated') {
        this.setColor();
      }
    });
  }

  private setColor(): void {
    this.layoutModel = this.layoutService.getLayout() || new LayoutModel();
    if (this.layoutModel.Color) {
      const color = this.colorService.getColor(this.layoutModel.Color);
      this.bgClass = color?.bgClass + ' ' + color?.sideBarTextClass;
    }
  }

  ngOnInit(): void {}
}
