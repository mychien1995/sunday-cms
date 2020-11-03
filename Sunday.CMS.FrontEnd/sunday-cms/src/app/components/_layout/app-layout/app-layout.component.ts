import { OnInit, Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppHeaderComponent } from '@components/_layout';

@Component({
  selector: 'app-layout',
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ApplicationLayoutComponent implements OnInit {
  currentView = 'default';
  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        const data = this.activatedRoute.snapshot.firstChild.data;
        if (data?.view) {
          this.currentView = data.view;
        } else {
          this.currentView = 'default';
        }
      }
    });
  }
  ngOnInit(): void {}
}
