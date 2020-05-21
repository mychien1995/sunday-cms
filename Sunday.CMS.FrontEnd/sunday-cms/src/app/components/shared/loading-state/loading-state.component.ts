import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
@Component({
  selector: 'app-loading-state',
  styleUrls: ['./loading-state.component.scss'],
  templateUrl: './loading-state.component.html',
  encapsulation: ViewEncapsulation.None
})

export class LoadingStateComponent implements OnInit {
  constructor(public clientState: ClientState) {

  }

  ngOnInit(): void {

  }
}
