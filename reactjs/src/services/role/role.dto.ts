import { EntityDto } from "../base/base.dto";

export interface PermissionDto extends EntityDto<number> {
  name: string;
}

export interface RoleDto extends EntityDto<string> {
  name: string;
  permissions: PermissionDto[];
}

export interface CreateRoleDto {
  name: string;
  permissionIds: number[];
}

export interface UpdateRoleDto extends EntityDto<string> {
  name: string;
  permissionIds: number[];
}
