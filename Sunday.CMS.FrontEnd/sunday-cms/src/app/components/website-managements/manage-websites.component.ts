import {
  Component,
  OnInit,
  ViewEncapsulation,
  TemplateRef,
} from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import {
  LayoutManagementService,
  WebsiteManagementService,
} from '@services/index';
import { LayoutItem, LayoutList, WebsiteList } from '@models/index';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-manage-websites',
  templateUrl: './manage-websites.component.html',
  providers: [NgbModalConfig, NgbModal],
})
export class ManageWebsiteComponent implements OnInit {
  websiteList: WebsiteList = new WebsiteList();
  layoutLookup: LayoutItem[] = [];
  activeId: string;

  constructor(
    private websiteService: WebsiteManagementService,
    private layoutService: LayoutManagementService,
    private clientState: ClientState,
    private modalService: NgbModal,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.getWebsites();
    this.getLayouts();
  }

  getWebsites(query?: any): void {
    this.clientState.isBusy = true;
    this.websiteService.getWebsites(query).subscribe((res) => {
      this.websiteList = <WebsiteList>res;
      this.clientState.isBusy = false;
    });
  }

  getLayouts() {
    this.layoutService.getLayouts({ PageSize: 1000 }).subscribe((res) => {
      this.layoutLookup = res.Layouts;
    });
  }

  deleteWebsite(websiteId: string, template: any): void {
    this.activeId = websiteId;
    this.modalService.open(template);
  }

  getLayout(layoutId: string): string {
    return this.layoutLookup.find((l) => l.Id === layoutId)?.LayoutName;
  }

  confirmDelete() {
    if (this.activeId) {
      this.clientState.isBusy = true;
      this.websiteService.deleteWebsite(this.activeId).subscribe((res) => {
        if (res.Success) {
          this.toastr.success('Website Deleted');
          this.modalService.dismissAll();
          this.getWebsites();
        }
        this.clientState.isBusy = false;
      });
    }
  }

  activate(websiteId: string, activated: boolean): void {
    this.clientState.isBusy = true;
    this.websiteService.activateWebsite(websiteId).subscribe((res) => {
      if (res.Success) {
        this.toastr.success(
          activated ? 'Organization Deactivated' : 'Organization Activated'
        );
        this.getWebsites();
      }
      this.clientState.isBusy = false;
    });
  }
}
