import { AxiosRequestConfig } from "axios";
import http from "../base/http.service";
import { NoContentDto } from "../base/base.dto";
import {
  UserDto,
  CreateUserDto,
  UpdateProfileDto,
  PasswordResetTokenDto,
  ResetPasswordDto,
  SendEmailChangeEmailDto,
  ConfirmEmailDto,
  ConfirmNewEmailDto,
  ChangePasswordDto,
} from "./account.dto";

const services = {
  Register(dto: CreateUserDto, config?: AxiosRequestConfig<any> | undefined) {
    return http.post<UserDto>("Account/Register", dto, config);
  },
  GetUserInformation(config?: AxiosRequestConfig<any> | undefined) {
    return http.get<UserDto>(`Account/GetUserInformation`, config);
  },
  UpdateProfile(
    dto: UpdateProfileDto,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return http.put<UserDto>("Account/UpdateProfile", dto, config);
  },
  DeleteAccount(config?: AxiosRequestConfig<any> | undefined) {
    return http.delete<NoContentDto>("Account/DeleteAccount", config);
  },
  SendPasswordResetEmail(
    dto: PasswordResetTokenDto,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return http.post<NoContentDto>("Account/SendPasswordResetEmail", dto,config);
  },
  ResetPassword(
    dto: ResetPasswordDto,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return http.post<NoContentDto>("Account/ResetPassword",dto, config);
  },
  SendEmailChangeEmail(
    dto: SendEmailChangeEmailDto,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return http.post<NoContentDto>("Account/SendEmailChangeEmail", dto,config);
  },
  ConfirmEmail(
    dto: ConfirmEmailDto,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return http.post<NoContentDto>("Account/ConfirmEmail", dto, config);
  },
  ConfirmNewEmail(
    dto: ConfirmNewEmailDto,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return http.post<NoContentDto>("Account/ConfirmNewEmail",dto, config);
  },
  ChangePassword(
    dto: ChangePasswordDto,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return http.post<NoContentDto>("Account/ChangePassword",dto, config);
  },
};

export default services;
