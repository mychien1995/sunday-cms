import { OnInit, Component } from '@angular/core';
import { AppHeaderComponent } from '@components/_layout';
import { ContentTree, ContentTreeNode, LayoutModel } from '@core/models';
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
  constructor(
    private iconService: IconService,
    private contentTreeService: ContentTreeService
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

  ngOnInit(): void {}
}
