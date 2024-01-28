export interface LoginDto {
  email: string;
  password: string;
}

export interface AccessTokenDto {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiration: Date;
  refreshTokenExpiration: Date;
}

export interface RefreshTokenDto {
  token: string;
}
