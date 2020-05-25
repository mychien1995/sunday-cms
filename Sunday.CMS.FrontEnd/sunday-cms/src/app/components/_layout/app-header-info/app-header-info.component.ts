import { OnInit, Component, ViewEncapsulation } from '@angular/core';
import { LayoutService, AuthenticationService } from '@services/index';
import { Router } from '@angular/router';
import { DefaultLogo } from '@core/constants';
import * as $ from 'jquery/dist/jquery.min.js';

@Component({
  selector: 'app-header-info',
  styleUrls: ['./app-header-info.component.scss'],
  templateUrl: './app-header-info.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class AppHeaderInfoComponent implements OnInit {
  public userName: string;
  public fullName: string;
  public avatar: string;
  constructor(
    private layoutService: LayoutService,
    private authService: AuthenticationService,
    private router: Router
  ) {
    this.layoutService.layoutBus.subscribe((data) => {
      if (data.event === 'user-updated') {
        this.loadUserProfile();
      }
    });
  }

  ngOnInit(): void {
    this.loadUserProfile();
  }

  logout(): void {
    this.authService.clearToken();
    this.router.navigate(['/login']);
  }

  redirect(link: string): void {
    this.router.navigate([link]);
    this.toggleDropdown();
  }

  toggleDropdown(): void {
    const $menu = $('#headerProfileMenu');
    if ($menu.hasClass('show')) {
      $menu.removeClass('show');
    } else {
      $menu.addClass('show');
    }
  }

  loadUserProfile() {
    const user = this.authService.getUser();
    if (user) {
      this.userName = user.Username;
      this.fullName = user.Fullname;
      const avatar = user.AvatarLink || DefaultLogo;
      this.avatar = avatar;
    }
  }
}
