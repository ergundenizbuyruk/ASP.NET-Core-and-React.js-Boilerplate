import { AxiosRequestConfig } from "axios";
import http from "../base/http.service";
import { RoleDto, CreateRoleDto, UpdateRoleDto, PermissionDto } from "./role.dto";
import { NoContentDto } from "../base/base.dto";

const services = {
  Get(id: string, config?: AxiosRequestConfig<any> | undefined) {
    return http.get<RoleDto>(`Role/Get/${id}`, config);
  },
  GetAll(config?: AxiosRequestConfig<any> | undefined) {
    return http.get<RoleDto[]>(`Role/GetAll`, config);
  },
  Create(dto: CreateRoleDto, config?: AxiosRequestConfig<any> | undefined) {
    return http.post<RoleDto>("Role/Create", dto, config);
  },
  Update(dto: UpdateRoleDto, config?: AxiosRequestConfig<any> | undefined) {
    return http.put<RoleDto>("Role/Update", dto, config);
  },
  Delete(id: string, config?: AxiosRequestConfig<any> | undefined) {
    return http.delete<NoContentDto>(`Role/Delete/${id}`, config);
  },
  GetAllPermissions(config?: AxiosRequestConfig<any> | undefined) {
    return http.get<PermissionDto[]>(`Role/GetAllPermissions`, config);
  },
};

export default services;
