import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { ContentField, TemplateField } from '@models/index';
import { FileUploadService } from '@services/index';

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
    this.fieldsChange.emit(val);
  }
  @Output()
  fieldsChange: EventEmitter<any> = new EventEmitter<any>();
  @Input() isEditable = true;

  previewLink: string;

  constructor(private fileService: FileUploadService) {}

  onSelectImage(event: any): void {
    const file: File = event.target.files && <File>event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = (ev: any) => {
        this.previewLink = ev.target.result;
      };
      this.fileService.uploadBlob('images', file).subscribe((res) => {
        this.innerField.value = res.BlobIdentifier;
        this.fieldsChange.emit(this.innerField);
      });
    }
  }

  ngOnInit(): void {}
}
