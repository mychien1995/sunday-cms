import { OnInit, Component } from '@angular/core';
import { IconService, TemplateManagementService } from '@services/index';
import { ContentField, ContentModel, TemplateItem } from '@models/index';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-content-detail',
  templateUrl: './content-detail.component.html',
  styleUrls: ['./content-detail.component.scss'],
})
export class ContentDetailComponent implements OnInit {
  content: ContentModel = new ContentModel();
  template: TemplateItem = new TemplateItem();
  fields: ContentField[] = [];
  isSubmitted = false;

  constructor(
    private iconService: IconService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    templateService: TemplateManagementService
  ) {
    this.activatedRoute.data.subscribe((data: { content: ContentModel }) => {
      if (!data.content || !data.content.Success) {
        this.router.navigate(['/manage-contents']);
        return;
      }
      if (data.content) {
        this.content = data.content;
        templateService.getFields(data.content.TemplateId).subscribe((res) => {
          this.fields = res.Data.map(
            (f) =>
              <ContentField>{
                field: f,
                value: data.content.Fields.find(
                  (v) => v.TemplateFieldId === f.Id
                )?.FieldValue,
              }
          );
        });
      }
    });
  }
  ngOnInit(): void {}

  getIcon(): string {
    return this.iconService.getIcon(this.content.Template.Icon);
  }
}
