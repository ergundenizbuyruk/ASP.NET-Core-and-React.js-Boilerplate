import axios, { AxiosRequestConfig } from "axios";
import { HttpResponse, ResponseDto } from "./base.dto";

declare module "axios" {
  export interface AxiosRequestConfig {
    blockUI?: boolean;
    overrideError?: boolean;
  }
}

async function post<TResponse>(
  url: string,
  data: any,
  config?: AxiosRequestConfig<any> | undefined
): Promise<HttpResponse<ResponseDto<TResponse>>> {
  try {
    const response = await axios.post(url, data, config);
    return { result: response.data as ResponseDto<TResponse> };
  } catch (error: any) {
    return { error: error?.response?.data };
  }
}

async function get<TResponse>(
  url: string,
  config?: AxiosRequestConfig<any> | undefined
): Promise<HttpResponse<ResponseDto<TResponse>>> {
  try {
    const response = await axios.get(url, config);
    return { result: response.data as ResponseDto<TResponse> };
  } catch (error: any) {
    return { error: error?.response?.data };
  }
}

async function put<TResponse>(
  url: string,
  data: any,
  config?: AxiosRequestConfig<any> | undefined
): Promise<HttpResponse<ResponseDto<TResponse>>> {
  try {
    const response = await axios.put(url, data, config);
    return { result: response.data as ResponseDto<TResponse> };
  } catch (error: any) {
    return { error: error?.response?.data };
  }
}

async function remove<TResponse>(
  url: string,
  config?: AxiosRequestConfig<any> | undefined
): Promise<HttpResponse<ResponseDto<TResponse>>> {
  try {
    const response = await axios.delete(url, config);
    return { result: response.data as ResponseDto<TResponse> };
  } catch (error: any) {
    return { error: error?.response?.data };
  }
}

async function patch<TResponse>(
  url: string,
  data: any,
  config?: AxiosRequestConfig<any> | undefined
): Promise<HttpResponse<ResponseDto<TResponse>>> {
  try {
    const response = await axios.patch(url, data, config);
    return { result: response.data as ResponseDto<TResponse> };
  } catch (error: any) {
    return { error: error?.response?.data };
  }
}

export default {
  get,
  post,
  put,
  delete: remove,
  patch,
};
