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

export interface UserProfile {
  username: string;
  email: string;
  createdAt: string;
  roles: string[];
}

export type SessionDto = {
  isLoggedIn: boolean;
  token: string | null;
  user: UserProfile | null;
};

