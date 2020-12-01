import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ContentTreeSelectorComponent } from '@components/contents/_layout/app-content-tree/content-tree-selector.component';
import {
  ContentModel,
  ContentTree,
  Rendering,
  RenderingValue,
} from '@models/index';
import {
  ContentService,
  ContentTreeService,
  RenderingService,
} from '@services/index';
import { DatasourceSelectorDialogComponent } from './datasource-selector.component';

@Component({
  selector: 'app-rendering-editor-dialog',
  templateUrl: './rendering-editor-dialog.component.html',
})
export class RenderingEditorDialogComponent implements OnInit {
  rendering: Rendering;
  content: ContentModel;
  renderingValue: RenderingValue;
  contentTree: ContentTree = new ContentTree();
  parameters: KeyValue[] = [{ Key: '', Value: '' }];
  callback: (renderingValue: RenderingValue) => any;
  @ViewChild(ContentTreeSelectorComponent)
  contentTreeRef: ContentTreeSelectorComponent;
  constructor(
    private contentService: ContentService,
    private renderingService: RenderingService,
    private contentTreeService: ContentTreeService,
    private dialogService: MatDialog
  ) {}

  ngOnInit(): void {}

  load(
    renderingValue: RenderingValue,
    callback: (renderingValue: RenderingValue) => any
  ): void {
    this.renderingValue = { ...renderingValue };
    this.callback = callback;
    this.renderingService
      .getById(renderingValue.RenderingId)
      .toPromise()
      .then((res) => (this.rendering = res));
    if (renderingValue.Datasource) {
      this.contentService.get(renderingValue.Datasource).subscribe((res) => {
        this.content = res;
        this.contentTreeService
          .getTreeByPath(this.content.Path)
          .subscribe((tree) => {
            this.contentTree = tree;
            this.contentTreeRef.tree = tree;
            this.contentTreeRef.activeNode = this.contentTreeRef.searchInTree(
              this.content.Id
            );
          });
      });
    }
    if (
      renderingValue.Parameters &&
      Object.keys(renderingValue.Parameters).length !== 0
    ) {
      this.parameters = [];
      for (const prop in renderingValue.Parameters) {
        if (renderingValue.Parameters.hasOwnProperty(prop)) {
          this.parameters.push(<KeyValue>{
            Key: prop,
            Value: renderingValue.Parameters[prop],
          });
        }
      }
      this.parameters.push({ Key: '', Value: '' });
    }
  }

  onBlur(keyValue: KeyValue): void {
    const last = this.parameters[this.parameters.length - 1];
    if (
      (!keyValue.Key || keyValue.Key.trim().length === 0) &&
      keyValue !== last
    ) {
      const index = this.parameters.indexOf(keyValue);
      this.parameters.splice(index, 1);
    }
    if (!(!last.Key || last.Key.trim().length === 0)) {
      this.parameters.push({ Key: '', Value: '' });
    }
  }
  onSubmit(): void {
    const param = {};
    this.parameters
      .filter((kv) => kv.Key && kv.Value)
      .forEach((item) => (param[item.Key] = item.Value));
    this.renderingValue.Parameters = param;
    if (this.rendering.IsRequireDatasource) {
      const selectedContent = this.contentTreeRef.activeNode;
      if (!selectedContent) {
        return;
      }
      this.renderingValue.Datasource = selectedContent.Id;
    }
    this.callback(this.renderingValue);
    this.dialogService.closeAll();
  }
}

class KeyValue {
  Key: string;
  Value: string;
}
