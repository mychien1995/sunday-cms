import { OnInit, Component } from '@angular/core';
import * as $ from 'jquery/dist/jquery.min.js';

@Component({
  selector: 'app-header-logo',
  templateUrl: './app-header-logo.component.html'
})

export class AppHeaderLogoComponent implements OnInit {


  ngOnInit(): void {
  }


  toggleSidebar(): void {
    var $this = $('.close-sidebar-btn');
    var cls = $this.attr("data-class");
    $(".app-container").toggleClass(cls);
    $this.hasClass("is-active") ? $this.removeClass("is-active") : $this.addClass("is-active")
  }
}
