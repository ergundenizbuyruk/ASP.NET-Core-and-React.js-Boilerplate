export interface HttpResponse<T> {
  [x: string]: any;
  result?: T;
  error?: ErrorDto;
}

export interface ResponseDto<T> {
  data: T;
  statusCode: number;
  error: ErrorDto;
}

export interface ErrorDto {
  errors: string[];
  isShow: boolean;
}

export interface NoContentDto {}
