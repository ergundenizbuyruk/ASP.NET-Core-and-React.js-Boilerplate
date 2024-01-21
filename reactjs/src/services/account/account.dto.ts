export interface UserDto {
  id: string;
  userName: string;
  email: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  isActive: boolean;
}

export interface CreateUserDto {
  userName: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
}

export interface UpdateProfileDto {
  firstName: string;
  lastName: string;
  phoneNumber: string;
}

export interface PasswordResetTokenDto {
  email: string;
}

export interface ResetPasswordDto {
  userId: string;
  token: string;
  newPassword: string;
}

export interface SendEmailChangeEmailDto {
  newEmail: string;
}

export interface ConfirmEmailDto {
  userId: string;
  token: string;
}

export interface ConfirmNewEmailDto {
  oldEmail: string;
  token: string;
  newEmail: string;
}

export interface ChangePasswordDto {
  currentPassword: string;
  newPassword: string;
}
