import type { LoginRequest, RegisterRequest, TokenResponse, UserProfile } from "../../types/auth";
import { api } from "../api";

export async function registerUser(data: RegisterRequest){
  const res = await api.post<TokenResponse>("/account/register", data);
  return res.data;
}

export async function loginUser(data: LoginRequest){
  const res = await api.post<TokenResponse>("/account/login", data);
  return res.data;
}

export async function logoutUser(){
  await api.post("/account/logout");
}

export async function getUserProfile(){
  const res = await api.get<UserProfile>("/account/profile");
  return res.data;
}


export async function getSession(): Promise<UserProfile | null> {
    const res = await api.get<UserProfile | null>("/account/session");
    return res.data;
}