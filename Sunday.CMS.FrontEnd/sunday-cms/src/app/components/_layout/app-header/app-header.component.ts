import { OnInit, Component } from '@angular/core';
import { AppHeaderInfoComponent, AppHeaderLogoComponent, AppHeaderQuickLinkComponent } from '@components/_layout';

@Component({
  selector: 'app-header',
  templateUrl: './app-header.component.html'
})

export class AppHeaderComponent implements OnInit {


  ngOnInit(): void {
  }

}
