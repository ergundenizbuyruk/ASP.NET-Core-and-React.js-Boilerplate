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
  try {
    const response = await apiClient.get<ResponseDto<T>>(url, config);
    return response.data;
  } catch (error: any) {
    return error?.response?.data as ResponseDto<T>;
  }
}

async function post<T>(
  url: string,
  body: any,
  config?: AxiosRequestConfig
): Promise<ResponseDto<T>> {
  try {
    const response = await apiClient.post<ResponseDto<T>>(url, body, config);
    return response.data;
  } catch (error: any) {
    return error?.response?.data as ResponseDto<T>;
  }
}

async function put<T>(
  url: string,
  body: any,
  config?: AxiosRequestConfig
): Promise<ResponseDto<T>> {
  try {
    const response = await apiClient.put<ResponseDto<T>>(url, body, config);
    return response.data;
  } catch (error: any) {
    return error?.response?.data as ResponseDto<T>;
  }
}

async function del<T>(
  url: string,
  config?: AxiosRequestConfig
): Promise<ResponseDto<T>> {
  try {
    const response = await apiClient.delete<ResponseDto<T>>(url, config);
    return response.data;
  } catch (error: any) {
    return error?.response?.data as ResponseDto<T>;
  }
}

export default {
  get,
  post,
  put,
  delete: del,
};
