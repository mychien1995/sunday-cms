import { OnInit, Component } from '@angular/core';
import { AppHeaderComponent } from '@components/_layout';
import { LayoutService, ClientState } from '@services/index';
import { LayoutModel, NavigationTree } from '@models/index';

@Component({
  selector: 'app-navigation',
  templateUrl: './app-navigation.component.html',
  styleUrls: ['./app-navigation.component.scss'],
})
export class AppNavigationComponent implements OnInit {
  navigationTree: NavigationTree;
  layoutModel: LayoutModel;
  constructor(layoutService: LayoutService, public clientState: ClientState) {
    this.navigationTree = layoutService.getNavigation() || new NavigationTree();
    this.layoutModel = layoutService.getLayout() || new LayoutModel();
    layoutService.layoutBus.subscribe((data) => {
      if (data.event === 'navigation-updated') {
        this.navigationTree = layoutService.getNavigation();
      }
      if (data.event === 'layout-updated') {
        this.layoutModel = layoutService.getLayout();
      }
    });
  }

  ngOnInit(): void {}
}
