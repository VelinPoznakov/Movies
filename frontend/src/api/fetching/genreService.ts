import type { CreateGenreRequest, GenreResponse, UpdateGenreRequest } from "../../types/genreTypes";
import { api } from "../api";

export async function GetAllGenresAsync(){
  const res = await api.get<GenreResponse[]>("/genre");
  return res.data;
}

export async function GetGenreByIdAsync(id: number){
  const res = await api.get<GenreResponse>(`/genre/${id}`);
  return res.data;
}

export async function CreateGenreAsync(data: CreateGenreRequest){
  const res = await api.post<GenreResponse>("/genre", data);
  return res.data;
}

export async function UpdateGenreAsync(id: number, data: UpdateGenreRequest){
  const res = await api.put<GenreResponse>(`/genre/${id}`, data);
  return res.data;
}

export async function DeleteGenreAsync(id: number){
  await api.delete(`/genre/${id}`);
}