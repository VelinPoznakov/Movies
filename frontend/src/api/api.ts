import axios from "axios";
import type { TokenResponse } from "../types/auth";

export const api = axios.create({
  baseURL: "http://localhost:5173/api",
  withCredentials: true,
});


api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});


api.interceptors.response.use(
  (res) => res,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        const refreshResponse = await api.post<TokenResponse>(
          "/account/refresh-token",
          {
            token: localStorage.getItem("token"),
          }
        );

        localStorage.setItem("token", refreshResponse.data.token);

        originalRequest.headers.Authorization =
          `Bearer ${refreshResponse.data.token}`;

        return api(originalRequest);
      } catch {
        localStorage.clear();
        window.location.href = "/login";
      }
    }

    return Promise.reject(error);
  }
);
