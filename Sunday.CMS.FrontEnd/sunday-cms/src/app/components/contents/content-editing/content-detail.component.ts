import { OnInit, Component } from '@angular/core';
import {
  ClientState,
  ContentService,
  IconService,
  TemplateManagementService,
} from '@services/index';
import {
  ContentField,
  ContentFieldItem,
  ContentModel,
  ContentVersion,
  TemplateItem,
} from '@models/index';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-content-detail',
  templateUrl: './content-detail.component.html',
  styleUrls: ['./content-detail.component.scss'],
})
export class ContentDetailComponent implements OnInit {
  content: ContentModel = new ContentModel();
  activeVersion: ContentVersion;
  template: TemplateItem = new TemplateItem();
  fields: ContentField[] = [];
  isSubmitted = false;
  showMenu = false;
  contentStatuses = {
    draft: 1,
    published: 2,
  };

  constructor(
    private iconService: IconService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private clientState: ClientState,
    private contentService: ContentService,
    private toastService: ToastrService,
    private templateService: TemplateManagementService,
    private modalService: NgbModal
  ) {
    this.activatedRoute.data.subscribe((data: { content: ContentModel }) => {
      if (!data.content || !data.content.Success) {
        this.router.navigate(['/manage-contents']);
        return;
      }
      if (data.content) {
        this.content = data.content;
        this.bindContentData();
      }
    });
  }
  ngOnInit(): void {}

  reload(): void {
    this.contentService.get(this.content.Id, this.activeVersion?.VersionId).subscribe((res) => {
      if (res.Success) {
        this.content = res;
        this.bindContentData();
      }
    });
  }

  get isEditable(): boolean {
    return (
      this.activeVersion &&
      this.activeVersion.Status !== this.contentStatuses.published
    );
  }

  get versionStatus(): string {
    if (!this.activeVersion) {
      return '';
    }
    return this.getVersionStatus(this.activeVersion.Status);
  }

  getVersionStatus(code: number): string {
    for (const prop in this.contentStatuses) {
      if (this.contentStatuses[prop] === code) {
        return prop;
      }
    }
    return '';
  }

  bindContentData() {
    if (this.content.SelectedVersion) {
      this.activeVersion = this.content.Versions.find(
        (v) => v.VersionId === this.content.SelectedVersion
      );
    } else {
      this.activeVersion = this.content.Versions.find((v) => v.IsActive);
    }
    this.templateService.getFields(this.content.TemplateId).subscribe((res) => {
      this.fields = res.Data.map(
        (f) =>
          <ContentField>{
            field: f,
            value: this.content.Fields.find((v) => v.TemplateFieldId === f.Id)
              ?.FieldValue,
          }
      );
    });
  }

  toggleMenu(): void {
    this.showMenu = !this.showMenu;
  }

  getIcon(): string {
    return this.iconService.getIcon(this.content.Template.Icon);
  }

  createNewVersion(): void {
    this.clientState.isBusy = true;
    this.contentService
      .newVersion(this.content.Id, this.activeVersion.VersionId)
      .subscribe(
        (res) => {
          if (res.Success) {
            this.toastService.success('New Version Created');
            this.showMenu = false;
            this.activeVersion = null;
            this.reload();
          }
          this.clientState.isBusy = false;
          this.modalService.dismissAll();
        },
        (ex) => {
          this.clientState.isBusy = false;
          this.modalService.dismissAll();
        }
      );
  }

  openDialog(template: any): void {
    this.showMenu = false;
    this.modalService.open(template);
  }

  publishContent(): void {
    this.clientState.isBusy = true;
    this.contentService.publish(this.content.Id).subscribe(
      (res) => {
        if (res.Success) {
          this.toastService.success(`${this.content.DisplayName} published`);
          this.showMenu = false;
          this.reload();
        }
        this.clientState.isBusy = false;
        this.modalService.dismissAll();
      },
      (ex) => {
        this.clientState.isBusy = false;
        this.modalService.dismissAll();
      }
    );
  }

  switchVersion(versionId: string) {
    this.router.navigate([`manage-contents/${this.content.Id}/${versionId}`]);
    this.showMenu = false;
  }

  saveContent(): void {
    this.isSubmitted = true;
    if (
      this.fields.filter(
        (f) => f.field.IsRequired && (!f.value || f.value.trim().length === 0)
      ).length === 0
    ) {
      const contentFields = this.fields.map((f) => {
        let contentField = this.content.Fields.find(
          (cf) => cf.TemplateFieldId === f.field.Id
        );
        if (!contentField) {
          contentField = <ContentFieldItem>{
            TemplateFieldId: f.field.Id,
          };
        }
        contentField.FieldValue = f.value ? f.value : null;
        return contentField;
      });
      const content = <ContentModel>{
        Id: this.content.Id,
        TemplateId: this.content.TemplateId,
        ParentId: this.content.ParentId,
        ParentType: this.content.ParentType,
        Name: this.content.Name,
        DisplayName: this.content.DisplayName,
        Fields: contentFields,
        Versions: [
          <ContentVersion>{
            VersionId: this.activeVersion.VersionId,
            IsActive: true,
          },
        ],
      };
      this.clientState.isBusy = true;
      this.contentService.update(content).subscribe(
        (res) => {
          if (res.Success) {
            this.toastService.success(`${this.content.DisplayName} updated`);
            this.showMenu = false;
            this.reload();
          }
          this.clientState.isBusy = false;
        },
        (ex) => (this.clientState.isBusy = false)
      );
    }
  }
}
