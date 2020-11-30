import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { ContentField, TemplateField } from '@models/index';
import { FileUploadService, FileService } from '@services/index';

@Component({
  selector: 'app-image-renderer',
  templateUrl: './image-renderer.component.html',
})
export class ImageRendererComponent implements OnInit {
  innerField: ContentField;
  @Input()
  get field(): ContentField {
    return this.innerField;
  }
  set field(val: ContentField) {
    this.innerField = val;
    this.fieldChange.emit(val);
    if (val.value) {
      this.fileService.previewImage(val.value).subscribe((res) => {
        this.previewLink = res.link;
      });
    } else {
      this.previewLink = '';
    }
  }
  @Output()
  fieldChange: EventEmitter<any> = new EventEmitter<any>();
  @Input() isEditable = true;

  previewLink: string;

  constructor(
    private fileUploadService: FileUploadService,
    private fileService: FileService
  ) {}

  onSelectImage(event: any): void {
    const file: File = event.target.files && <File>event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = (ev: any) => {
        this.previewLink = ev.target.result;
      };
      this.fileUploadService.uploadBlob('images', file).subscribe((res) => {
        this.innerField.value = res.BlobIdentifier;
        this.fieldChange.emit(this.innerField);
      });
    }
  }

  ngOnInit(): void {}
}
