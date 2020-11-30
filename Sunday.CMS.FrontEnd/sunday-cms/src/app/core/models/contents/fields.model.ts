export class LinkValue {
  LinkText: string;
  Url: string;
  Target?: string;
  Hint?: string;
}

export class RenderingValue {
  RenderingId: string;
  Datasource?: string;
  Parameters: any = {};
}
