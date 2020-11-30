import {
  Component,
} from '@angular/core';
import {
  ContentTreeNode,
} from '@core/models';
import { ContentTreeComponent } from './content-tree.component';

@Component({
  selector: 'app-content-tree-selector',
  templateUrl: './content-tree.component.html',
  styleUrls: ['./content-tree.component.scss'],
})
export class ContentTreeSelectorComponent extends ContentTreeComponent {
  loadInitialData(): void {
    this.getTemplateIcons().then(() => {});
  }
  onSelectNode(node: ContentTreeNode): void {
    this.activeNode = node;
  }
  openContextMenu(ev: any, node: ContentTreeNode): void {}
}
