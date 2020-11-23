export class ApiResponse {
  Success: boolean;
  Errors: string[];
}

export class ListApiResponse<T> extends ApiResponse {
  Total: number;
  List: T[] = [];
}

export class GenericApiResponse<T> extends ApiResponse {
  Data: T;
}
