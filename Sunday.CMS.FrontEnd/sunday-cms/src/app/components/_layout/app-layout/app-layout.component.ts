import { OnInit, Component, ViewEncapsulation } from '@angular/core';
import { AppHeaderComponent } from '@components/_layout';

@Component({
  selector: 'app-layout',
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class ApplicationLayoutComponent implements OnInit {


  ngOnInit(): void {
  }

}
