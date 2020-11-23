import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ClientState, RenderingService } from '@services/index';
import { ListApiResponse, Rendering } from '@models/index';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-manage-renderings',
  templateUrl: './manage-rendering.component.html',
  providers: [NgbModalConfig, NgbModal],
})
export class ManageRenderingComponent implements OnInit {
  renderings: ListApiResponse<Rendering> = new ListApiResponse<Rendering>();
  query?: any = {};
  activeId: string;
  searchForm: FormGroup = new FormGroup({
    Text: new FormControl(this.query?.Text || ''),
  });
  constructor(
    private renderingService: RenderingService,
    private clientState: ClientState,
    private modalService: NgbModal,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.getRenderings();
  }

  getRenderings(): void {
    this.clientState.isBusy = true;
    this.renderingService.getRenderings(this.query).subscribe((res) => {
      this.renderings = <ListApiResponse<Rendering>>res;
      this.clientState.isBusy = false;
    });
  }

  doSearch(param?: any): void {
    this.query = { ...this.query, ...param };
    this.getRenderings();
  }

  deleteRendering(id: string, template: any): void {
    this.activeId = id;
    this.modalService.open(template);
  }

  confirmDelete() {
    if (this.activeId) {
      this.clientState.isBusy = true;
      this.renderingService.delete(this.activeId).subscribe((res) => {
        if (res.Success) {
          this.toastr.success('Rendering Deleted');
          this.modalService.dismissAll();
          this.getRenderings();
        }
        this.clientState.isBusy = false;
      });
    }
  }
}
