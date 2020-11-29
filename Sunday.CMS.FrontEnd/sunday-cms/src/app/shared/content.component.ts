import { AddRenderingComponent } from '@components/contents/renderings/add-rendering.component';
import { ManageRenderingComponent } from '@components/contents/renderings/manage-rendering.component';
import { RenderingResolver } from '@components/contents/renderings/rendering.resolver';
import {
  ContentDashboardComponent,
  ContentTreeComponent,
  AppContentSidebarComponent,
  TemplateSelectorComponent,
  TemplateSelectorDialogComponent,
  InitialContentCreatorComponent,
  FieldsRendererComponent,
  SingleLineTextRendererComponent,
  ContentDetailComponent,
  ContentResolver,
  RichTextRendererComponent,
  MultilineTextRendererComponent,
  ContentVersionResolver,
  ContentRenameDialogComponent,
  EntityAccessDialogComponent,
  EntityAccessEditorComponent,
  InlineTemplateSelectorDialogComponent,
  TemplateSelectorButtonComponent,
  ImageRendererComponent,
  LinkRendererComponent,
} from 'app/components';
export const ContentComponents = [
  ContentDashboardComponent,
  ContentTreeComponent,
  AppContentSidebarComponent,
  TemplateSelectorComponent,
  TemplateSelectorDialogComponent,
  InitialContentCreatorComponent,
  FieldsRendererComponent,
  SingleLineTextRendererComponent,
  ContentDetailComponent,
  RichTextRendererComponent,
  MultilineTextRendererComponent,
  ContentRenameDialogComponent,
  EntityAccessDialogComponent,
  EntityAccessEditorComponent,
  ManageRenderingComponent,
  AddRenderingComponent,
  InlineTemplateSelectorDialogComponent,
  TemplateSelectorButtonComponent,
  ImageRendererComponent,
  LinkRendererComponent
];

export const ContentResolvers = [
  ContentResolver,
  ContentVersionResolver,
  RenderingResolver,
];
