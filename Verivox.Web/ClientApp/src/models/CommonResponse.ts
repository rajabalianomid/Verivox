interface CommonResponse<T> {
  Message: string;
  IsError: boolean;
  Result: T;
}
