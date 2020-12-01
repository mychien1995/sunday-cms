import {
  OnInit,
  Component,
  Input,
  Output,
  EventEmitter,
  ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ContentService, ContentTreeService } from '@services/index';
import { ContentField, ContentModel, ContentTree } from '@models/index';
import { DatasourceSelectorDialogComponent } from './partials/datasource-selector.component';
import { ContentTreeSelectorComponent } from '@components/contents/_layout/app-content-tree/content-tree-selector.component';

@Component({
  selector: 'app-multilist-renderer',
  templateUrl: './multilist-renderer.component.html',
  styleUrls: ['./multilist-renderer.component.scss'],
})
export class MultilistRendererComponent implements OnInit {
  innerField: ContentField;
  @Input()
  get field(): ContentField {
    return this.innerField;
  }
  set field(val: ContentField) {
    this.innerField = val;
    if (this.innerField.value) {
      this.contentService
        .getMultiples(this.innerField.value)
        .subscribe((res) => {
          this.contents = this.innerField.value.map((id) =>
            res.List.find((c) => c.Id === id)
          );
        });
    }
    this.fieldChange.emit(val);
  }
  @Output()
  fieldChange: EventEmitter<any> = new EventEmitter<any>();
  @Input() isEditable = true;
  @Input() websiteId: string;
  contents: ContentModel[] = [];
  contentTree: ContentTree = new ContentTree();
  selectedId: string;

  @ViewChild(ContentTreeSelectorComponent)
  contentTreeRef: ContentTreeSelectorComponent;
  constructor(
    private contentService: ContentService,
    private contentTreeService: ContentTreeService
  ) {}

  ngOnInit(): void {
    this.contentTreeService
      .getTreeByQuery({ WebsiteId: this.websiteId, Location: '/' })
      .subscribe((res) => {
        this.contentTree = res;
      });
  }

  moveItem(): void {
    const selectedNode = this.contentTreeRef.activeNode;
    if (selectedNode) {
      let arr = [];
      if (this.innerField.value) {
        arr = <string[]>this.innerField.value;
      }
      const index = arr.indexOf(selectedNode.Id);
      if (index === -1) {
        arr.push(selectedNode.Id);
        this.broadcastChange(arr);
        this.selectedId = null;
      }
    }
  }

  revertItem(): void {
    if (this.selectedId) {
      let arr = [];
      if (this.innerField.value) {
        arr = <string[]>this.innerField.value;
        const index = arr.indexOf(this.selectedId);
        if (index > -1) {
          arr.splice(index, 1);
          this.broadcastChange(arr);
          this.selectedId = null;
        }
      }
    }
  }

  moveUp(): void {
    if (this.selectedId) {
      let arr = [];
      if (this.innerField.value) {
        arr = <string[]>this.innerField.value;
        const index = arr.indexOf(this.selectedId);
        if (index > -1 && index !== 0) {
          const tmp = arr[index - 1];
          arr[index - 1] = this.selectedId;
          arr[index] = tmp;
          this.broadcastChange(arr);
        }
      }
    }
  }

  moveDown(): void {
    if (this.selectedId) {
      let arr = [];
      if (this.innerField.value) {
        arr = <string[]>this.innerField.value;
        const index = arr.indexOf(this.selectedId);
        if (index > -1 && index !== arr.length - 1) {
          const tmp = arr[index + 1];
          arr[index + 1] = this.selectedId;
          arr[index] = tmp;
          this.broadcastChange(arr);
        }
      }
    }
  }

  broadcastChange(arr: string[]) {
    this.innerField.value = arr;
    this.field = this.innerField;
  }

  selectItem(content: ContentModel) {
    this.selectedId = content.Id;
  }
}
