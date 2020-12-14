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
import { ContentDetailComponent } from '@components/contents/content-editing/content-detail.component';
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
import { Location } from '@angular/common';

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
  dragNode: ContentTreeNode;
  dragging = false;
  hoverNode: ContentTreeNode;
  hoverPosition = 0;
  @ViewChild('confirmDeleteDialog') deleteDialog: ElementRef;
  @ViewChild('confirmDropDialog') moveDialog: ElementRef;
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
    private contentBus: ContentBus,
    private location: Location
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
            this.tree.Roots.forEach((el) => {
              el.Open = true;
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
    this.tree.Roots.forEach((el) => {
      this.setLeafParent(el);
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
      const isContentDetail =
        this.activatedRoute.snapshot.firstChild.component ===
        ContentDetailComponent;
      if (!isContentDetail) {
        this.router.navigate([node.Link]);
      } else {
        this.contentBus.contentLinkSelect(node.Id);
        this.location.go(`/manage-contents/${node.Id}`);
      }
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
        ref.componentInstance.load(
          this.activeNode,
          this.getWebsiteId(this.activeNode),
          (id) => {
            this.reloadNode(this.activeNode, () => {
              const activeItem = this.activeNode.ChildNodes.find(
                (c) => c.Id === id
              );
              if (activeItem) {
                this.activeNode = activeItem;
              }
            });
          }
        );
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

  getWebsiteId(node: ContentTreeNode): string {
    if (node.Type == 2) {
      return node.Id;
    }
    if (node.Type == 1) {
      return null;
    }
    return this.getWebsiteId(node.ParentNode);
  }

  @HostListener('document:click', ['$event'])
  closeContextMenu(): void {
    this.contextMenu.hidden = true;
  }

  @HostListener('document:mousemove', ['$event'])
  onMouseMove(event): void {
    if (this.dragNode && this.dragging) {
      const dragNode = this.dragNode;
      const elements = document.elementsFromPoint(event.clientX, event.clientY);
      const currentElement = document.elementFromPoint(
        event.clientX,
        event.clientY
      );
      for (let i = 0; i < elements.length; i++) {
        const el = elements[i];
        const id = el.getAttribute('data-nodeid');
        if (id && id !== dragNode.Id) {
          const node = this.searchInTree(id);
          if (node && node.Type.toString() === '3') {
            this.hoverNode = node;
            const listItem = el.tagName === 'li' ? el : el.closest('li');
            const currentElementRect = currentElement.getBoundingClientRect();
            const currentPoint =
              currentElementRect.top + currentElementRect.height / 2;
            const hoverRect = listItem.getBoundingClientRect();
            const insideTop = hoverRect.top + hoverRect.height / 2 - 10;
            const insideBottom = hoverRect.top + hoverRect.height / 2 + 10;
            if (currentPoint < insideTop) {
              this.hoverPosition = -1;
            } else if (currentPoint > insideBottom) {
              this.hoverPosition = 1;
            } else {
              this.hoverPosition = 0;
            }
          }
        }
      }
    }
  }

  isContent(node: ContentTreeNode): boolean {
    return node.Type.toString() === this.parentTypeMappings.content.toString();
  }

  startDrag(item: ContentTreeNode) {
    if (item.Type.toString() === '3') {
      this.dragNode = item;
      this.dragging = true;
    }
  }

  stopDrag(ev) {
    const evt = ev;
    if (this.dragNode && this.hoverNode) {
      this.dragging = false;
      const ref = this.modalService.open(this.moveDialog);
      ref.result
        .then((_) => {
          evt.source._dragRef.reset();
          this.dragNode = null;
          this.hoverNode = null;
        })
        .catch((e) => {
          evt.source._dragRef.reset();
          this.dragNode = null;
          this.hoverNode = null;
        });
    } else {
      evt.source._dragRef.reset();
      this.dragNode = null;
      this.hoverNode = null;
    }
  }

  confirmMove() {
    if (this.hoverNode && this.dragNode) {
      const targetId = this.hoverNode.Id;
      const targetType = this.hoverNode.Type;
      let parentId = this.dragNode.ParentId || this.dragNode.ParentNode.Id;
      let parentType = this.dragNode.ParentNode.Type;
      if (this.hoverPosition === 0) {
        parentId = this.hoverNode.Id;
        parentType = this.hoverNode.Type;
        if (this.hoverNode.Id === this.dragNode.ParentId) {
          this.modalService.dismissAll();
          return;
        }
      }
      this.isTreeLoading = true;
      this.contentService
        .move(this.dragNode.Id, targetId, targetType, this.hoverPosition)
        .subscribe(
          (res) => {
            this.isTreeLoading = false;
            if (parentId !== this.dragNode.ParentNode.Id) {
              this.reloadNode(this.dragNode.ParentNode, () => {});
            }
            const parentNode = this.searchInTree(parentId);
            this.reloadNode(parentNode, () => {});
            this.modalService.dismissAll();
          },
          (ex) => (this.isTreeLoading = false)
        );
    }
  }
}
