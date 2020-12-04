import { OnInit, Component } from '@angular/core';
import { ClientState, ContentBus, ContentService } from '@services/index';
import { ContentModel } from '@models/index';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
@Component({
  selector: 'app-content-rename',
  templateUrl: './content-rename.component.html',
})
export class ContentRenameDialogComponent implements OnInit {
  content: ContentModel = new ContentModel();
  public contentForm: FormGroup = new FormGroup({
    Name: new FormControl(),
    DisplayName: new FormControl(),
  });
  constructor(
    private contentService: ContentService,
    private clientState: ClientState,
    private toastService: ToastrService,
    private dialogService: MatDialog,
    private contentBus: ContentBus
  ) {}
  ngOnInit(): void {}

  load(id: string) {
    this.clientState.isBusy = true;
    this.contentService.get(id).subscribe(
      (res) => {
        if (res.Success) {
          this.content = res;
          this.contentForm = new FormGroup({
            Name: new FormControl(this.content.Name, [Validators.required]),
            DisplayName: new FormControl(this.content.DisplayName, [
              Validators.required,
            ]),
          });
        }
        this.clientState.isBusy = false;
      },
      (ex) => (this.clientState.isBusy = false)
    );
  }

  onSubmit(): void {
    if (this.contentForm.valid) {
      this.clientState.isBusy = true;
      this.content.Name = this.contentForm.controls['Name'].value;
      this.content.DisplayName = this.contentForm.controls['DisplayName'].value;
      this.contentService.updateExplicit(this.content).subscribe(
        (res) => {
          this.clientState.isBusy = false;
          if (res.Success) {
            this.toastService.success('Content renamed');
            this.contentBus.contentUpdated(this.content);
            this.dialogService.closeAll();
          }
        },
        (ex) => (this.clientState.isBusy = false)
      );
    }
  }
}
