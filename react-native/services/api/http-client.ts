import { AxiosRequestConfig } from "axios";
import apiClient from "./axios-instance";

export interface ResponseDto<T> {
  data: T;
  statusCode: number;
  error: ErrorDto;
}

export interface ErrorDto {
  errors: string[];
  isShow: boolean;
}

async function get<T>(
  url: string,
  config?: AxiosRequestConfig
): Promise<ResponseDto<T>> {
  const response = await apiClient.get<ResponseDto<T>>(url, config);
  return response.data;
}

async function post<T>(
  url: string,
  body: any,
  config?: AxiosRequestConfig
): Promise<ResponseDto<T>> {
  const response = await apiClient.post<ResponseDto<T>>(url, body, config);
  return response.data;
}

async function put<T>(
  url: string,
  body: any,
  config?: AxiosRequestConfig
): Promise<ResponseDto<T>> {
  const response = await apiClient.put<ResponseDto<T>>(url, body, config);
  return response.data;
}

async function del<T>(
  url: string,
  config?: AxiosRequestConfig
): Promise<ResponseDto<T>> {
  const response = await apiClient.delete<ResponseDto<T>>(url, config);
  return response.data;
}

export default {
  get,
  post,
  put,
  delete: del,
};
