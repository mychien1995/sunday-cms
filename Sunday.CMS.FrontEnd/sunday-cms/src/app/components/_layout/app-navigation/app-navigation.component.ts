import { OnInit, Component } from '@angular/core';
import { AppHeaderComponent } from '@components/_layout';
import { LayoutService, ClientState } from '@services/index';
import { NavigationTree } from '@models/index';

@Component({
  selector: 'app-navigation',
  templateUrl: './app-navigation.component.html',
})
export class AppNavigationComponent implements OnInit {
  navigationTree: NavigationTree;
  constructor(private layoutService: LayoutService, public clientState : ClientState) {

    this.navigationTree = layoutService.getNavigation() || new NavigationTree();
    layoutService.layoutBus.subscribe((data) => {
      if (data.event === 'navigation-updated') {
        this.navigationTree = layoutService.getNavigation();
      }
    });
  }

  ngOnInit(): void {}
}
