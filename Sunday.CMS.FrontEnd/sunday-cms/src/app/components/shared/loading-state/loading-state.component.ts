import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
@Component({
  selector: 'loading-state',
  styleUrls: ['./loading-state.component.scss'],
  templateUrl: './loading-state.component.html',
  encapsulation: ViewEncapsulation.None
})

export class LoadingStateComponent implements OnInit {
  constructor(private clientState: ClientState) {

  }

  ngOnInit(): void {

  }
}
