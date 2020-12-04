import { Component, OnInit, ViewChild } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { IconService, TemplateManagementService } from '@services/index';
import { TemplateItem, TemplateList } from '@models/index';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { TemplateInfoComponent } from '../template-managements/partials/template-info.component';
import { TemplateFieldComponent } from '../template-managements/partials/template-fields.component';

@Component({
  selector: 'app-template-detail',
  templateUrl: './template-detail.component.html',
  providers: [NgbModalConfig, NgbModal],
})
export class TemplateDetailComponent implements OnInit {
  template: TemplateItem = new TemplateItem();
  isEdit: boolean;
  templateIcon = 'bg-happy-itmeo';
  activeTab = 1;
  submitted: boolean;

  @ViewChild('info') templateInfo: TemplateInfoComponent;
  @ViewChild('fields') templateFields: TemplateFieldComponent;

  constructor(
    private templateService: TemplateManagementService,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService,
    private iconService: IconService,
    private clientstate: ClientState,
    private router: Router
  ) {
    this.activatedRoute.data.subscribe((data: { template: TemplateItem }) => {
      if (data.template) {
        this.template = data.template;
        this.isEdit = true;
        this.templateIcon = iconService.getIcon(this.template.Icon);
      }
    });
  }

  onSubmit(): void {
    this.submitted = true;
    const infoValid = this.templateInfo.isValid();
    const fieldValid = this.templateFields.isValid();
    if (!infoValid) {
      this.activeTab = 1;
    } else if (!fieldValid) {
      this.activeTab = 2;
    } else {
      const fields = this.template.Fields.filter((f) => !f.IsPlaceholder);
      fields.forEach(function (item, index) {
        if (!item.SortOrder) {
          item.SortOrder = index + 1;
        }
      });
      const data = <TemplateItem>this.template;
      data.Fields = fields;
      this.clientstate.isBusy = true;
      const promise = this.isEdit
        ? this.templateService.updateTemplate(data)
        : this.templateService.createTemplate(data);
      promise.subscribe(
        (res) => {
          if (res.Success) {
            this.toastr.success('Template Saved');
            this.router.navigate(['/manage-templates']);
          }
          this.clientstate.isBusy = false;
        },
        (ex) => (this.clientstate.isBusy = false)
      );
    }
  }
  ngOnInit(): void {}
}
