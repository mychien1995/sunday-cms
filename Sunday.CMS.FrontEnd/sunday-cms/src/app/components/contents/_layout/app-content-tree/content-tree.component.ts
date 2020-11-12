import {
  OnInit,
  Component,
  HostListener,
  ViewChild,
  TemplateRef,
  ElementRef,
} from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
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
  ClientState,
  ContentService,
} from '@core/services';
import { LayoutService } from '@core/services';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
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
    private activatedRoute: ActivatedRoute
  ) {
    this.loadTree();
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        const data = this.activatedRoute.snapshot.firstChild.data;
        if (data?.view === 'content' && data?.content) {
          console.log('has content');
        }
      }
    });
  }

  loadTree(): void {
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
  closeContextMenu(ev: any): void {
    this.contextMenu.hidden = true;
  }

  isContent(node: ContentTreeNode): boolean {
    return node.Type.toString() === this.parentTypeMappings.content.toString();
  }

  ngOnInit(): void {}
}
