import { OnInit, Component } from '@angular/core';
import { LinkValue } from '@models/index';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-link-editor-dialog',
  templateUrl: './link-editor-dialog.component.html',
})
export class LinkEditorDialogComponent implements OnInit {
  link: LinkValue;
  editLink: LinkValue;
  submitted: boolean;
  callback: (link: LinkValue) => any;
  ngOnInit(): void {}
  constructor(private dialogService: MatDialog) {}

  load(link: LinkValue, callback: (link: LinkValue) => any): void {
    this.link = link;
    this.editLink = { ...link };
    this.callback = callback;
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.isValid(this.editLink)) {
      this.link = this.editLink;
      this.callback(this.editLink);
      const ref = this.dialogService.getDialogById('link_renderer');
      if (ref) {
        ref.close();
      }
    }
  }

  isValid = (link: LinkValue) =>
    link.LinkText &&
    link.Url &&
    link.Url.trim().length > 0 &&
    link.Url.trim().length > 0;
}
