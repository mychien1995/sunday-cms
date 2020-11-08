import { OnInit, Component, HostListener } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TemplateSelectorDialogComponent } from '@components/contents/content-creation/template-selector-dialog.component';
import { AppHeaderComponent } from '@components/_layout';
import {
  ContentTree,
  ContentTreeNode,
  ContextMenuItem,
  LayoutModel,
} from '@core/models';
import {
  IconService,
  ContentTreeService,
  TemplateManagementService,
} from '@core/services';
import { LayoutService } from '@core/services';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-content-tree',
  templateUrl: './content-tree.component.html',
  styleUrls: ['./content-tree.component.scss'],
})
export class ContentTreeComponent implements OnInit {
  parentTypeMappings = {
    organization: 1,
    website: 2,
    content: 3,
  };
  tree: ContentTree = new ContentTree();
  templateIconLookup = {};
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
    private dialogService: MatDialog,
    private templateService: TemplateManagementService,
    private router: Router
  ) {
    this.loadTree();
  }

  loadTree(): void {
    console.log('load tree');
    this.isTreeLoading = true;
    this.templateService
      .getTemplates({ PageSize: 1000 })
      .pipe(
        switchMap((res) => {
          res.Templates.forEach(
            (template) => (this.templateIconLookup[template.Id] = template.Icon)
          );
          return this.contentTreeService.getRoots();
        })
      )
      .subscribe(
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
  getNodeIcon(node: ContentTreeNode): string {
    const code =
      node.Type.toString() === this.parentTypeMappings.content.toString()
        ? this.templateIconLookup[node.Icon]
        : node.Icon;
    return this.iconService.getIcon(code);
  }

  reloadNode(node: ContentTreeNode): void {
    this.contentTreeService.getChilds(node).subscribe((res) => {
      if (res.Success) {
        node.ChildNodes = res.Nodes;
        node.Open = true;
      }
    });
  }

  expandNode(node: ContentTreeNode): void {
    if (node.Open) {
      node.Open = false;
      return;
    }
    this.reloadNode(node);
  }

  onSelectNode(node: ContentTreeNode): void {
    if (node.Type.toString() === this.parentTypeMappings.content.toString()) {
      this.router.navigate([node.Link]);
    }
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
        const currentNode = <ContentTreeNode>this.contextMenu.node;
        const ref = this.dialogService.open(TemplateSelectorDialogComponent, {
          minWidth: 800,
          disableClose: true,
        });
        ref.componentInstance.load(currentNode);
        ref.afterClosed().subscribe((res) => this.reloadNode(currentNode));
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
