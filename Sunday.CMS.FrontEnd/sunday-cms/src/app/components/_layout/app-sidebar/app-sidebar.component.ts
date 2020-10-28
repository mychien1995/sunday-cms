import { OnInit, Component } from '@angular/core';
import { AppHeaderComponent } from '@components/_layout';
import { LayoutModel } from '@core/models';
import { ColorService } from '@core/services';
import { LayoutService } from '@core/services';

@Component({
  selector: 'app-sidebar',
  templateUrl: './app-sidebar.component.html',
})
export class AppSidebarComponent implements OnInit {
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
