import {
  OnInit,
  Component,
  Input,
  Output,
  EventEmitter,
  HostListener,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  ContentField,
  Rendering,
  RenderingValue,
  TemplateField,
} from '@models/index';
import { RenderingSelectorDialogComponent } from './partials/rendering-selector-dialog.component';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { LayoutService, RenderingService } from '@services/index';
import { RenderingEditorDialogComponent } from './partials/rendering-editor-dialog.component';

@Component({
  selector: 'app-rendering-area-renderer',
  templateUrl: './rendering-area-renderer.component.html',
  styleUrls: ['./rendering-area-renderer.component.scss'],
})
export class RenderingAreaRenderComponent implements OnInit {
  renderings: RenderingValue[] = [];
  renderingLookup: Rendering[] = [];
  innerField: ContentField;
  @Input()
  get field(): ContentField {
    return this.innerField;
  }
  set field(val: ContentField) {
    this.innerField = val;
    if (val.value) {
      this.renderings = val.value.Renderings;
    } else {
      this.renderings = [];
    }
    this.fieldChange.emit(val);
  }
  @Output()
  fieldChange: EventEmitter<any> = new EventEmitter<any>();
  @Input() isEditable = true;
  @Input() websiteId: string;
  @Input() contentId: string;
  contextMenu: any;

  constructor(
    private dialogService: MatDialog,
    private renderingService: RenderingService,
    private layoutService: LayoutService
  ) {
    this.contextMenu = {
      hidden: true,
      element: {},
    };
    this.layoutService.layoutBus.subscribe((res) => {
      if (res.event && res.event === 'document:click') {
        this.closeContextMenu(res.data);
      }
    });
  }

  openDialog() {
    const ref = this.dialogService.open(RenderingSelectorDialogComponent, {
      minWidth: 600,
    });
    ref.componentInstance.load(
      (renderingVal) => {
        this.renderings.push(renderingVal);
        this.innerField.value = {
          Renderings: this.renderings,
        };
        this.fieldChange.emit(this.innerField);
      },
      this.websiteId,
      this.contentId
    );
  }

  openContextMenu(ev: any, rendering: RenderingValue): void {
    ev.preventDefault();
    this.contextMenu.hidden = false;
    this.contextMenu.element = ev.target;
    this.contextMenu.rendering = rendering;
  }

  ngOnInit(): void {
    this.renderingService
      .getRenderings({
        IsPageRendering: false,
        WebsiteId: this.websiteId,
        PageSize: 100000,
      })
      .subscribe((res) => {
        this.renderingLookup = res.List;
      });
  }

  closeContextMenu(event): void {
    if (
      this.contextMenu.element &&
      this.contextMenu.element != event.target
    ) {
      this.contextMenu.hidden = true;
      this.contextMenu.element = null;
    }
  }

  editRendering(): void {
    const rendering = this.contextMenu.rendering;
    if (!rendering) {
      return;
    }
    const ref = this.dialogService.open(RenderingEditorDialogComponent, {
      minWidth: 600,
    });
    ref.componentInstance.load(rendering, (val) => {
      const index = this.renderings.indexOf(rendering);
      this.renderings[index] = val;
    });
  }
  removeRendering(): void {
    const rendering = this.contextMenu.rendering;
    if (!rendering) {
      return;
    }
    const index = this.renderings.indexOf(rendering);
    if (index > -1) {
      this.renderings.splice(index, 1);
    }
  }
  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.renderings, event.previousIndex, event.currentIndex);
  }

  getRenderingName(id: string): string {
    return this.renderingLookup.find((r) => r.Id === id)?.RenderingName;
  }
}
