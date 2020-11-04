export class ApiResponse {
  Success: boolean;
  Errors: string[];
}

export class GenericApiResponse<T> extends ApiResponse {
  Data: T;
}
