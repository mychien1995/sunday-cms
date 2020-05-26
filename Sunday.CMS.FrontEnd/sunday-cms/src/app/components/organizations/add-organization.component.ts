import {
  Component,
  OnInit,
  ViewEncapsulation,
  TemplateRef,
} from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { Router, ActivatedRoute } from '@angular/router';
import { OrganizationDetailResponse } from '@models/index';

@Component({
  selector: 'app-add-organization',
  templateUrl: './add-organization.component.html',
})
export class AddOrganizationComponent implements OnInit {
  public formTitle = 'Create Organization';
  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    this.activatedRoute.data.subscribe((data: { organization: OrganizationDetailResponse }) => {
      if(data.organization){
        this.formTitle = 'Edit Organization';
      }
    });
  }
  ngOnInit(): void {}
}
