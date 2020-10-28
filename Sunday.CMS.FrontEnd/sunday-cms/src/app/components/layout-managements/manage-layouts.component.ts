import {
  Component,
  OnInit,
  ViewEncapsulation,
  TemplateRef,
} from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { LayoutManagementService } from '@services/index';
import { LayoutList } from '@models/index';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-manage-layouts',
  templateUrl: './manage-layouts.component.html',
  providers: [NgbModalConfig, NgbModal],
})
export class ManageLayoutComponent implements OnInit {
  layoutList: LayoutList = new LayoutList();
  activeId: string;

  constructor(
    private layoutService: LayoutManagementService,
    private clientState: ClientState,
    private modalService: NgbModal,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.getLayouts();
  }

  getLayouts(query?: any): void {
    this.clientState.isBusy = true;
    this.layoutService.getLayouts(query).subscribe((res) => {
      this.layoutList = <LayoutList>res;
      this.clientState.isBusy = false;
    });
  }

  deleteLayout(layoutId: string, template: any): void {
    this.activeId = layoutId;
    this.modalService.open(template);
  }

  confirmDelete() {
    if (this.activeId) {
      this.clientState.isBusy = true;
      this.layoutService.deleteLayout(this.activeId).subscribe((res) => {
        if (res.Success) {
          this.toastr.success('Layout Deleted');
          this.modalService.dismissAll();
          this.getLayouts();
        }
        this.clientState.isBusy = false;
      });
    }
  }
}
