import axios from "axios";
import { getAccessToken, setAccessToken } from "../accessToken";
import type { TokenResponse } from "../types/auth";

export const api = axios.create({
  baseURL: "http://localhost:5233/api",
  withCredentials: true,
});


export const refreshTokenApi = axios.create({
  baseURL: "http://localhost:5233/api",
  withCredentials: true,
})

api.interceptors.request.use((config) => {
  const token = getAccessToken();

  if(token){
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

let refreshPromise: Promise<string> | null = null;

async function refreshAccessToken(): Promise<string>{
  const res = await refreshTokenApi.post<TokenResponse>("/account/refresh-token");
  setAccessToken(res.data.token);
  return res.data.token;
}

api.interceptors.response.use(
  (response) => response,

  async (error) => {
    const originalRequest = error.config;

    if(!originalRequest) return Promise.reject(error);
    if(originalRequest.response.status !== 401) return Promise.reject(error);

    if(originalRequest._retry) return Promise.reject(error);
    originalRequest._retry = true;

    if((originalRequest.url ?? "").includes("/account/refresh-token")){
      return Promise.reject(error);
    }

    try{
      refreshPromise ??= refreshAccessToken();
      const newToken = await refreshPromise;
      refreshPromise = null;

      originalRequest.headers = originalRequest.headers ?? {};
      originalRequest.headers.Authorization = `Bearer ${newToken}`;
      return api(originalRequest);
      
    }catch(err){
      refreshPromise = null;
      setAccessToken(null);
      return Promise.reject(err);
    }


  }
)
