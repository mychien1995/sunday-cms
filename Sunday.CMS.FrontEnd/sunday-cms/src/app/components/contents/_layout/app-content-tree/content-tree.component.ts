import { OnInit, Component, HostListener } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TemplateSelectorDialogComponent } from '@components/contents/content-creation/template-selector-dialog.component';
import { AppHeaderComponent } from '@components/_layout';
import {
  ContentTree,
  ContentTreeNode,
  ContextMenuItem,
  LayoutModel,
} from '@core/models';
import { IconService, ContentTreeService } from '@core/services';
import { LayoutService } from '@core/services';

@Component({
  selector: 'app-content-tree',
  templateUrl: './content-tree.component.html',
  styleUrls: ['./content-tree.component.scss'],
})
export class ContentTreeComponent implements OnInit {
  tree: ContentTree = new ContentTree();
  isTreeLoading = false;
  contextMenu = {
    x: '0px',
    y: '0px',
    hidden: true,
    items: [],
    node: {},
  };
  constructor(
    private iconService: IconService,
    private contentTreeService: ContentTreeService,
    private dialogService: MatDialog
  ) {
    this.loadTree();
  }

  loadTree(): void {
    console.log('load tree');
    this.isTreeLoading = true;
    this.contentTreeService.getRoots().subscribe(
      (res) => {
        if (res.Success) {
          this.tree = res;
          this.tree.Roots.forEach((element) => {
            element.Open = true;
          });
        }
        this.isTreeLoading = false;
      },
      (ex) => (this.isTreeLoading = false)
    );
  }

  getIcon(code: string): string {
    return this.iconService.getIcon(code);
  }

  expandNode(node: ContentTreeNode): void {
    if (node.Open) {
      node.Open = false;
      return;
    }
    this.contentTreeService.getChilds(node).subscribe((res) => {
      if (res.Success) {
        node.ChildNodes = res.Nodes;
        node.Open = true;
      }
    });
  }

  openContextMenu(ev: any, node: ContentTreeNode): void {
    ev.preventDefault();
    this.contentTreeService.getContextMenu(node).subscribe((res) => {
      if (res.Success) {
        this.contextMenu.x = ev.clientX + 'px';
        this.contextMenu.y = ev.clientY + 'px';
        this.contextMenu.items = res.Items;
        this.contextMenu.hidden = false;
        this.contextMenu.node = node;
      }
    });
  }

  processCommand(item: ContextMenuItem): void {
    const command = item.Command;
    switch (command) {
      case 'createcontent':
        this.dialogService.open(TemplateSelectorDialogComponent, {
          minWidth: 800,
          disableClose: true,
        });
        break;
      default:
        break;
    }
  }

  @HostListener('document:click', ['$event'])
  closeContextMenu(ev: any): void {
    this.contextMenu.hidden = true;
  }

  ngOnInit(): void {}
}
