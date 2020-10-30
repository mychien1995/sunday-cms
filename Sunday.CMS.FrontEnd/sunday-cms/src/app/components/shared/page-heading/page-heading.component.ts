import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-page-heading',
  templateUrl: './page-heading.component.html',
})
export class PageHeadingComponent implements OnInit {
  @Input() title?: string;
  @Input() description?: string;
  @Input() icon?: string;

  constructor() {}

  ngOnInit(): void {}

  get displayIcon() {
    return this.icon || 'pe-7s-drawer';
  }
}
