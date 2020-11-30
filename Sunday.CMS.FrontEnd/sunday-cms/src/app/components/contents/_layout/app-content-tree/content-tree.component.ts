import {
  OnInit,
  Component,
  HostListener,
  ViewChild,
  ElementRef,
  Input,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';
import { TemplateSelectorDialogComponent } from '@components/contents/content-creation/template-selector-dialog.component';
import { ContentRenameDialogComponent } from '@components/contents/content-editing/content-rename.component';
import {
  ContentModel,
  ContentTree,
  ContentTreeNode,
  ContextMenuItem,
} from '@core/models';
import {
  IconService,
  ContentTreeService,
  TemplateManagementService,
  ClientState,
  ContentService,
  ContentBus,
} from '@core/services';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

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
  @Input()
  tree: ContentTree = new ContentTree();

  templateIconLookup = {};
  isTreeLoading = false;
  contextMenu = {
    x: '0px',
    y: '0px',
    hidden: true,
    items: [],
  };
  activeNode: ContentTreeNode;
  @ViewChild('confirmDeleteDialog') deleteDialog: ElementRef;
  constructor(
    private iconService: IconService,
    private contentTreeService: ContentTreeService,
    private dialogService: MatDialog,
    private templateService: TemplateManagementService,
    private router: Router,
    private clientState: ClientState,
    private contentService: ContentService,
    private toastr: ToastrService,
    private modalService: NgbModal,
    private activatedRoute: ActivatedRoute,
    private contentBus: ContentBus
  ) {}

  loadInitialData(): void {
    this.activatedRoute.firstChild.data.subscribe((res) => {
      if (res.content) {
        const contentModel = <ContentModel>res.content;
        this.loadTreeByPath((<ContentModel>res.content).Path, contentModel.Id);
      } else {
        this.loadTree();
      }
    });
    this.contentBus.contentBus.subscribe((content) => {
      if ((<ContentModel>content).Id === this.activeNode?.Id) {
        this.reloadNode(this.activeNode, () => {});
        this.activeNode.Name = (<ContentModel>content).DisplayName;
      }
    });
  }

  ngOnInit(): void {
    this.loadInitialData();
  }

  getTemplateIcons(): Promise<any> {
    return this.templateService
      .getTemplates({ PageSize: 1000 })
      .toPromise()
      .then((res) => {
        res.Templates.forEach(
          (template) => (this.templateIconLookup[template.Id] = template.Icon)
        );
      });
  }

  loadTreeByPath(path: string, activeId: string): void {
    this.isTreeLoading = true;
    this.getTemplateIcons().then(() => {
      this.contentTreeService.getTreeByPath(path).subscribe(
        (res) => {
          if (res.Success) {
            this.tree = res;
            this.activeNode = this.searchInTree(activeId);
            this.setParentNode();
          }
          this.isTreeLoading = false;
        },
        () => (this.isTreeLoading = false)
      );
    });
  }

  searchInTree(activeId: string): ContentTreeNode {
    const nodes = this.tree.Roots;
    const queue = [...nodes];
    while (queue.length > 0) {
      const current = queue.pop();
      if (current.Id === activeId) {
        return current;
      }
      queue.push(...current.ChildNodes);
    }
    return undefined;
  }

  loadTree(): void {
    this.isTreeLoading = true;
    this.getTemplateIcons().then(() =>
      this.contentTreeService.getRoots().subscribe(
        (res) => {
          if (res.Success) {
            this.tree = res;
            this.tree.Roots.forEach((element) => {
              element.Open = true;
            });
            this.setParentNode();
          }
          this.isTreeLoading = false;
        },
        () => (this.isTreeLoading = false)
      )
    );
  }

  setParentNode(): void {
    this.tree.Roots.forEach((element) => {
      this.setLeafParent(element);
    });
  }

  setLeafParent(node: ContentTreeNode): void {
    node.ChildNodes.forEach((child) => {
      child.ParentNode = node;
      this.setLeafParent(child);
    });
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

  reloadNode(node: ContentTreeNode, callback: () => any): void {
    this.contentTreeService.getChilds(node).subscribe((res) => {
      if (res.Success) {
        node.ChildNodes = res.Nodes;
        node.ChildNodes.forEach((child) => (child.ParentNode = node));
        node.Open = true;
        callback();
      }
    });
  }

  expandNode(node: ContentTreeNode): void {
    if (node.Open) {
      node.Open = false;
      return;
    }
    this.reloadNode(node, () => {});
  }

  onSelectNode(node: ContentTreeNode): void {
    if (this.isContent(node)) {
      this.router.navigate([node.Link]);
      this.activeNode = node;
    }
  }

  openContextMenu(ev: any, node: ContentTreeNode): void {
    ev.preventDefault();
    this.activeNode = node;
    this.contentTreeService.getContextMenu(node).subscribe((res) => {
      if (res.Success) {
        this.contextMenu.x = ev.clientX + 'px';
        this.contextMenu.y = ev.clientY + 'px';
        this.contextMenu.items = res.Items;
        this.contextMenu.hidden = false;
      }
    });
  }

  processCommand(item: ContextMenuItem): void {
    const command = item.Command;
    switch (command) {
      case 'createcontent':
        const ref = this.dialogService.open(TemplateSelectorDialogComponent, {
          minWidth: 800,
          disableClose: true,
        });
        ref.componentInstance.load(this.activeNode, (id) => {
          this.reloadNode(this.activeNode, () => {
            const activeItem = this.activeNode.ChildNodes.find(
              (c) => c.Id === id
            );
            if (activeItem) {
              this.activeNode = activeItem;
            }
          });
        });
        break;
      case 'deletecontent':
        this.modalService.open(this.deleteDialog);
        break;
      case 'renamecontent':
        const renameDialogRef = this.dialogService.open(
          ContentRenameDialogComponent,
          {
            minWidth: 300,
            disableClose: true,
          }
        );
        renameDialogRef.componentInstance.load(this.activeNode.Id);
        break;
      default:
        break;
    }
  }

  confirmDelete(): void {
    if (this.activeNode) {
      this.clientState.isBusy = true;
      this.contentService.delete(this.activeNode.Id).subscribe((res) => {
        this.clientState.isBusy = false;
        if (res.Success) {
          this.toastr.success(this.activeNode.Name + ' deleted');
          this.modalService.dismissAll();
          const parentNode = this.activeNode.ParentNode;
          if (parentNode) {
            this.reloadNode(parentNode, () => {
              this.activeNode = parentNode;
              if (this.isContent(parentNode)) {
                this.router.navigate([
                  `/manage-contents/${this.activeNode.Id}`,
                ]);
                return;
              }
              this.router.navigate([`/manage-contents/`]);
            });
          }
          this.router.navigate([`/manage-contents/`]);
          this.activeNode = null;
        }
      });
    }
  }

  @HostListener('document:click', ['$event'])
  closeContextMenu(): void {
    this.contextMenu.hidden = true;
  }

  isContent(node: ContentTreeNode): boolean {
    return node.Type.toString() === this.parentTypeMappings.content.toString();
  }
}
