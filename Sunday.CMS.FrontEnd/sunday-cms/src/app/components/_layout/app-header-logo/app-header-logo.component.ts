import { OnInit, Component } from '@angular/core';
import { LayoutService } from '@core/services';
import { LayoutModel } from '@core/models';
import * as $ from 'jquery/dist/jquery.min.js';

@Component({
  selector: 'app-header-logo',
  templateUrl: './app-header-logo.component.html',
  styleUrls: ['./app-header-logo.component.scss'],
})
export class AppHeaderLogoComponent implements OnInit {
  layoutModel: LayoutModel;
  constructor(layoutService: LayoutService) {
    this.layoutModel = layoutService.getLayout();
    if (!this.layoutModel) {
      this.layoutModel = new LayoutModel();
      layoutService.refresh();
    }
    layoutService.layoutBus.subscribe((data) => {
      if (data.event === 'layout-updated') {
        this.layoutModel = layoutService.getLayout();
      }
    });
  }

  ngOnInit(): void {}

  toggleSidebar(): void {
    const $this = $('.close-sidebar-btn');
    const cls = $this.attr('data-class');
    $('.app-container').toggleClass(cls);
    $this.hasClass('is-active')
      ? $this.removeClass('is-active')
      : $this.addClass('is-active');
  }
}
