import { OnInit, Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import {
  ContentField,
  ContentFieldItem,
  ContentModel,
  ContentTreeNode,
  TemplateField,
} from '@models/index';
import {
  ClientState,
  ContentService,
  TemplateManagementService,
} from '@services/index';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-content-creator-dialog',
  templateUrl: './initial-content-creator.component.html',
  styleUrls: ['./initial-content-creator.component.scss'],
})
export class InitialContentCreatorComponent implements OnInit {
  templateId: string;
  parent: ContentTreeNode;
  fields: ContentField[] = [];
  name: string;
  displayName: string;
  isLoading = false;
  isSubmitted = false;
  onCreated: (id: string) => any;
  constructor(
    private templateService: TemplateManagementService,
    private contentService: ContentService,
    private clienState: ClientState,
    private toastService: ToastrService,
    private dialogService: MatDialog,
    private router: Router
  ) {}
  ngOnInit(): void {}

  load(
    templateId: string,
    parent: ContentTreeNode,
    onClose: (id: string) => any
  ): void {
    this.parent = parent;
    this.templateId = templateId;
    this.isLoading = true;
    this.onCreated = onClose;
    this.templateService.getFields(templateId).subscribe(
      (res) => {
        this.fields = res.Data.filter((c) => c.IsRequired).map(
          (c) => <ContentField>{ field: c }
        );
        this.isLoading = false;
      },
      (ex) => (this.isLoading = false)
    );
  }

  getSortOrder(): number {
    const siblings = [...this.parent.ChildNodes];
    if (siblings.length === 0) {
      return 0;
    }
    const dumpNode = <ContentTreeNode>{
      Name: this.name,
    };
    siblings.push(dumpNode);
    const sorted = siblings.sort(this.sortTreeNode);
    return sorted.indexOf(dumpNode);
  }

  sortTreeNode(nodeA: ContentTreeNode, nodeB: ContentTreeNode) {
    if (nodeA.Name.toLowerCase() < nodeB.Name.toLowerCase()) {
      return -1;
    } else if (nodeA.Name.toLowerCase() > nodeB.Name.toLowerCase()) {
      return 1;
    } else {
      return 0;
    }
  }
  onSubmit(): void {
    this.isSubmitted = true;
    if (
      this.name &&
      this.displayName &&
      this.name.trim().length > 0 &&
      this.displayName.trim().length > 0 &&
      this.fields.filter((f) => !f.value).length === 0
    ) {
      const content = <ContentModel>{
        TemplateId: this.templateId,
        ParentId: this.parent.Id,
        ParentType: this.parent.Type,
        Name: this.name,
        DisplayName: this.displayName,
        Fields: this.fields.map(
          (f) =>
            <ContentFieldItem>{
              FieldValue: f.value,
              TemplateFieldId: f.field.Id,
            }
        ),
        SortOrder: this.getSortOrder(),
      };
      this.clienState.isBusy = true;
      this.contentService.create(content).subscribe(
        (res) => {
          if (res.Success) {
            this.toastService.success(`${this.displayName} created`);
            this.dialogService.closeAll();
            this.onCreated(res['Id'].toString());
            this.router.navigate([`manage-contents/${res['Id']}`]);
          }
          this.clienState.isBusy = false;
        },
        (ex) => (this.clienState.isBusy = false)
      );
    }
  }
}
