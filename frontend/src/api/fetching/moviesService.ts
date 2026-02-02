import type { CreateMovieRequest, MovieResponse, UpdateMovieRequest } from "../../types/moviesTypes";
import { api } from "../api";

export async function GetAllMoviesAsync() {
  const res = await api.get<MovieResponse[]>("/movie");
  return res.data;
}

export async function GetMovieByIdAsync(id: number){
  const res = await api.get<MovieResponse>(`/movie/${id}`);
  return res.data;
}

export async function CreateMovieAsync(data: CreateMovieRequest){
  const res = await api.post<MovieResponse>("/movie", data);
  return res.data;
}

export async function UpdateMovieAsync(id: number, data: UpdateMovieRequest){
  const res = await api.put<MovieResponse>(`/movie/${id}`, data);
  return res.data;
}

export async function DeleteMovieAsync(id: number){
  await api.delete<void>(`/movie/${id}`);
}

