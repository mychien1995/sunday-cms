import {
  Component,
  OnInit,
  ViewEncapsulation,
  TemplateRef,
  Output,
  EventEmitter,
} from '@angular/core';

import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-organizations-filter',
  templateUrl: './organization-filter.component.html',
})
export class OrganizationFilterComponent implements OnInit {
  @Output() doSearch: EventEmitter<any> = new EventEmitter();

  searchForm: FormGroup = new FormGroup({
    Text: new FormControl(''),
  });

  ngOnInit(): void {}

  onSubmit(formValue: any): void {
    if (!this.searchForm.valid) {
      return;
    }
    this.doSearch.emit(formValue);
  }
}
