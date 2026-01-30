import type { LoginRequest, RegisterRequest, TokenResponse, UserProfile } from "../../types/auth";
import { api } from "../api";
import type { AxiosError } from "axios";

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

export async function getUserProfile(): Promise<UserProfile | null> {
  try{
    const res = await api.get<UserProfile>("/account/profile");
    return res.data;
  }catch(err){
    if((err as AxiosError)?.response?.status === 401) return null;
    throw err;
  }
}