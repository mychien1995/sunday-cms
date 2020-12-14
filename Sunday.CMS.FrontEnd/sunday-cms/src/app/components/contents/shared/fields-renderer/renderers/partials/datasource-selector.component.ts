import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ContentTreeSelectorComponent } from '@components/contents/_layout/app-content-tree/content-tree-selector.component';
import { ContentTree, ContentTreeNode, Rendering } from '@models/index';
import { ContentTreeService } from '@services/index';

@Component({
  selector: 'app-datasource-selector',
  templateUrl: './datasource-selector.component.html',
})
export class DatasourceSelectorDialogComponent implements OnInit {
  contentTree: ContentTree = new ContentTree();
  callback: (datasourceId: string) => any;
  constructor(
    private contentTreeService: ContentTreeService,
    private dialogService: MatDialog
  ) {}

  @ViewChild(ContentTreeSelectorComponent)
  contentTreeRef: ContentTreeSelectorComponent;
  load(
    location: string,
    websiteId: string,
    callback: (datasourceId: string) => any
  ) {
    this.callback = callback;
    this.contentTreeService
      .getTreeByQuery({
        Location: location,
        WebsiteId: websiteId,
      })
      .subscribe((res) => {
        this.contentTree = res;
      });
  }

  onSubmit(): void {
    const selectedContent = this.contentTreeRef.activeNode;
    if (selectedContent) {
      this.callback(selectedContent.Id);
      const ref = this.dialogService.getDialogById('datasource_selector');
      if (ref) {
        ref.close();
      } else {
        this.dialogService.closeAll();
      }
    }
  }
  ngOnInit(): void {}
}
