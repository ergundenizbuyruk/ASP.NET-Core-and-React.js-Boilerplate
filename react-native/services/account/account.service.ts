import { AxiosRequestConfig } from "axios";
import httpClient from "../api/http-client";
import { UserDto } from "./account.dto";
import { CreateUserDto, UpdateProfileDto } from "./account.schemas";

const services = {
  Register(dto: CreateUserDto, config?: AxiosRequestConfig<any> | undefined) {
    return httpClient.post<UserDto>("Account/register", dto, config);
  },
  GetProfile(config?: AxiosRequestConfig<any> | undefined) {
    return httpClient.get<UserDto>("Account/profile", config);
  },
  UpdateProfile(
    dto: UpdateProfileDto,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return httpClient.put<UserDto>("Account/profile", dto, config);
  },
  DeleteAccount(config?: AxiosRequestConfig<any> | undefined) {
    return httpClient.delete("Account/profile", config);
  },
  EmailConfirmationTokenRequest(
    email: string,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return httpClient.post(
      "Account/email-confirmation-request",
      { email },
      config
    );
  },
  ResetPasswordRequest(
    email: string,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return httpClient.post("Account/reset-password-request", { email }, config);
  },
  ResetPassword(
    userId: string,
    token: string,
    newPassword: string,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return httpClient.post(
      "Account/reset-password",
      { userId, token, newPassword },
      config
    );
  },

  ConfirmEmail(
    email: string,
    token: string,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return httpClient.post("Account/confirm-email", { email, token }, config);
  },

  ChangeEmailRequest(
    newEmail: string,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return httpClient.post(
      "Account/change-email-request",
      { newEmail },
      config
    );
  },

  ChangeEmail(
    oldEmail: string,
    token: string,
    newEmail: string,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return httpClient.post(
      "Account/change-email",
      { oldEmail, token, newEmail },
      config
    );
  },

  ChangePassword(
    currentPassword: string,
    newPassword: string,
    config?: AxiosRequestConfig<any> | undefined
  ) {
    return httpClient.post(
      "Account/change-password",
      { currentPassword, newPassword },
      config
    );
  },
};

export default services;
