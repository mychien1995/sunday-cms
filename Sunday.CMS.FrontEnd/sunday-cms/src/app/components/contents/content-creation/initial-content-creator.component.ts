import { OnInit, Component } from '@angular/core';
import { ContentField, TemplateField } from '@models/index';
import { TemplateManagementService } from '@services/index';

@Component({
  selector: 'app-content-creator-dialog',
  templateUrl: './initial-content-creator.component.html',
  styleUrls: ['./initial-content-creator.component.scss'],
})
export class InitialContentCreatorComponent implements OnInit {
  templateId: string;
  fields: ContentField[] = [];
  isLoading = false;
  constructor(private templateService: TemplateManagementService) {}
  ngOnInit(): void {}

  load(templateId: string): void {
    this.templateId = templateId;
    this.isLoading = true;
    this.templateService.getFields(templateId).subscribe(
      (res) => {
        this.fields = res.Data.filter((c) => c.IsRequired).map(
          (c) => <ContentField>{ field: c }
        );
        this.isLoading = false;
      },
      (ex) => (this.isLoading = false)
    );
  }

  onSubmit(): void {}
}
