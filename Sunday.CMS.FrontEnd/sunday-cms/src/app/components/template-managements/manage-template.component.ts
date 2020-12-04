import { Component, OnInit } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { IconService, TemplateManagementService } from '@services/index';
import { TemplateList } from '@models/index';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-manage-templates',
  templateUrl: './manage-template.component.html',
  providers: [NgbModalConfig, NgbModal],
})
export class ManageTemplateComponent implements OnInit {
  constructor(
    private templateService: TemplateManagementService,
    private clientState: ClientState,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private iconService: IconService
  ) {}
  templateList: TemplateList = new TemplateList();
  activeId: string;
  query?: any = { PageSize: 20 };
  searchForm: FormGroup = new FormGroup({
    Text: new FormControl(this.query?.Text || ''),
    IsPageTemplate: new FormControl(this.query?.IsPageTemplate),
  });

  ngOnInit(): void {
    this.getTemplates();
  }

  doSearch(param?: any): void {
    this.query = { ...this.query, ...param };
    this.getTemplates();
  }

  getTemplates(): void {
    this.clientState.isBusy = true;
    this.templateService.getTemplates(this.query).subscribe((res) => {
      this.templateList = <TemplateList>res;
      this.clientState.isBusy = false;
    });
  }

  deleteTemplate(templateId: string, template: any): void {
    this.activeId = templateId;
    this.modalService.open(template);
  }

  getIcon(code: string): string {
    return this.iconService.getIcon(code);
  }

  confirmDelete() {
    if (this.activeId) {
      this.clientState.isBusy = true;
      this.templateService.deleteTemplate(this.activeId).subscribe((res) => {
        if (res.Success) {
          this.toastr.success('Template Deleted');
          this.modalService.dismissAll();
          this.getTemplates();
        }
        this.clientState.isBusy = false;
      });
    }
  }
}
