import { AxiosRequestConfig } from "axios";
import httpClient from "../api/http-client";
import { AccessTokenDto, RefreshTokenDto } from "./auth.dto";

const services = {
  login(
    email: string,
    password: string,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return httpClient.post<AccessTokenDto>(
      "Auth/login",
      { email, password },
      config
    );
  },
  revoke(refreshToken: string, config?: AxiosRequestConfig<any> | undefined) {
    return httpClient.post(`Auth/revoke`, { token: refreshToken }, config);
  },
  refresh(dto: RefreshTokenDto, config?: AxiosRequestConfig<any> | undefined) {
    return httpClient.post<AccessTokenDto>("Auth/refresh", dto, config);
  },
};

export default services;
