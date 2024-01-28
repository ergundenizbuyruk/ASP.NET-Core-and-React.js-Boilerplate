import { AxiosRequestConfig } from "axios";
import http from "../base/http.service";
import { NoContentDto } from "../base/base.dto";
import { AccessTokenDto, RefreshTokenDto, LoginDto } from "./auth.dto";

const services = {
  CreateToken(dto: LoginDto, config?: AxiosRequestConfig<any> | undefined) {
    return http.post<AccessTokenDto>("Auth/CreateToken", dto, config);
  },
  RevokeRefreshToken(config?: AxiosRequestConfig<any> | undefined) {
    return http.delete<NoContentDto>(`Auth/RevokeRefreshToken`, config);
  },
  CreateTokenByRefreshToken(
    dto: RefreshTokenDto,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return http.post<AccessTokenDto>(
      "Auth/CreateTokenByRefreshToken",
      dto,
      config
    );
  },
};

export default services;
