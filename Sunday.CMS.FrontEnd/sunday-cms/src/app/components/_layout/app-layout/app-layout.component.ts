import {
  OnInit,
  Component,
  ViewEncapsulation,
  ViewChild,
  ElementRef,
  HostListener,
} from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppHeaderComponent } from '@components/_layout';
import * as $ from 'jquery/dist/jquery.min.js';
import { debounce, LayoutService } from '@core/index';

@Component({
  selector: 'app-layout',
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ApplicationLayoutComponent implements OnInit {
  currentView = 'default';
  isTrackingSidebar = false;
  @ViewChild('handleBar') handleBar: ElementRef;
  @ViewChild('sideBar', { read: ElementRef }) sideBar: ElementRef;
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private layoutService: LayoutService
  ) {
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

  startTracking(ev: any): void {
    this.isTrackingSidebar = true;
  }

  @HostListener('window:mouseup', ['$event'])
  stopTracking(event: any): void {
    this.isTrackingSidebar = false;
  }

  @HostListener('document:click', ['$event'])
  broadcastClick(event: any): void {
    this.layoutService.layoutBus.emit({
      event: 'document:click',
      data: event,
    });
  }

  @HostListener('window:mousemove', ['$event'])
  moveSidebar(event: any): void {
    const $this = this;
    if ($this.isTrackingSidebar) {
      const width = event.screenX;
      const sideElement = $this.sideBar.nativeElement;
      const $sidebar = $(sideElement).find('.app-sidebar');
      $sidebar[0].style.width = width + 'px';
    }
  }
}
