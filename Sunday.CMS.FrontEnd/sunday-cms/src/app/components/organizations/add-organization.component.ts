import {
  Component,
  OnInit,
  ViewEncapsulation,
  TemplateRef,
} from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-add-organization',
  templateUrl: './add-organization.component.html',
})
export class AddOrganizationComponent implements OnInit {
  public formTitle: string;
  constructor(private router: Router, private activatedRoute: ActivatedRoute) {}
  ngOnInit(): void {}
}
