export interface ErrorMessage {
  message: string;
}
export interface ErrorResponse {
  errorId: string;
  errors: ErrorMessage[];
}

export interface HttpResponse<T> {
  [x: string]: any;
  result?: T;
  error?: ErrorResponse;
}

export interface ResponseDto<T> {
  data: T;
  statusCode: number;
  error: ErrorDto;
}

export interface ErrorDto {
  errors: string[];
}

export interface NoContentDto {}
