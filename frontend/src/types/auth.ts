// src/types/auth.ts
export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

export interface TokenResponse {
  isLoggedIn: boolean;
  token: string;
}

export interface RefreshTokenRequest {
  token: string;
  refreshToken: string;
}

export interface UserProfile {
  username: string;
  email: string;
}
