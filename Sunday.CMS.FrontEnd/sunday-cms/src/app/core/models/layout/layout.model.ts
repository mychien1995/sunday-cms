export class NavigationTree {
  Sections?: NavigationSection[] = [];
  CreatedDate?: number;
}

export class NavigationSection {
  Section?: string;
  Items?: NavigationItem[] = [];
}

export class NavigationItem {
  Title?: string;
  Link?: string;
  IconClass?: string;
}
