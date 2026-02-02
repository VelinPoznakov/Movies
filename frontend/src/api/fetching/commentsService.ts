import type { CommentResponse, CreateCommentsRequest, UpdateCommentRequest } from "../../types/commentType";
import { api } from "../api";

export async function GetAllCommentsByMovieIdAsync(movieId: number){
  const res = await api.get<CommentResponse[]>(`/comment/${movieId}`);
  return res.data;
}

export async function CreateCommentsAsync(movieId: number, data: CreateCommentsRequest){
  const res = await api.post<CommentResponse>(`/comment/${movieId}`, data);
  return res.data;
}

export async function UpdateCommentAsync(id: number, data: UpdateCommentRequest){
  const res = await api.put<CommentResponse>(`/comment/${id}`, data);
  return res.data;
}

export async function DeleteCommentAsync(id: number){
  await api.delete(`/comment/${id}`);
}