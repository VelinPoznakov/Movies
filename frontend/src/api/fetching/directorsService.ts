import type { CreateDirectorRequest, DirectorResponse, UpdateDirectorRequest } from "../../types/directorsTypes";
import { api } from "../api";

export async function GetAllDirectorsAsync(){
  const res  = await api.get<DirectorResponse[]>("/director");
  return res.data;
}

export async function GetDirectorByIdAsync(id: number){
  const res = await api.get<DirectorResponse>(`/director/${id}`);
  return res.data;
}

export async function CreateDirectorAsync(data: CreateDirectorRequest){
  const res = await api.post<DirectorResponse>(`/director`, data);
  return res.data;
}

export async function UpdateDirectorAsync(id: number, data: UpdateDirectorRequest){
  const res = await api.put<DirectorResponse>(`/director/${id}`, data);
  return res.data;
}

export async function DeleteDirectorAsync(id: number){
  await api.delete(`/director/${id}`);
}
