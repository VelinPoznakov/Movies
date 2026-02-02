import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { CreateCommentsAsync, DeleteCommentAsync, GetAllCommentsByMovieIdAsync, UpdateCommentAsync } from "../../api/fetching/commentsService";
import message from "antd/es/message";
import type { CreateCommentsRequest, UpdateCommentRequest } from "../../types/commentType";

export function useGetAllCommentsByMovieId(movieId: number){

  return useQuery({
    queryKey: ["comments", movieId],
    queryFn: () => GetAllCommentsByMovieIdAsync(movieId),
    staleTime: 1000 * 60 * 5,
  })
}

type CreateCommentsData = {
  movieId: number;
  data: CreateCommentsRequest;
}

export function useCreateComment(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: ({movieId, data}: CreateCommentsData) => CreateCommentsAsync(movieId, data),
    onSuccess: (_res, vars) => {
      qc.invalidateQueries({ queryKey: ["comments", vars.movieId] });
    },
    onError: (error) => {
      message.error(`Failed to create comment. ${error.message}`);
    }
  })
}

type UpdateCommentData = {
  movieId: number;
  id: number;
  data: UpdateCommentRequest;
}

export function useUpdateComment(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: ({id, data}: UpdateCommentData) => UpdateCommentAsync(id, data),
    onSuccess: (_res, vars) => {
      qc.invalidateQueries({ queryKey: ["comments", vars.movieId] });
    },
    onError: (error) => {
      message.error(`Failed to update comment. ${error.message}`);
    }
  })
}

type DeleteCommentVars = {
  movieId: number;
  id: number;
};

export function useDeleteComment() {
  const qc = useQueryClient();

  return useMutation({
    mutationFn: ({ id }: DeleteCommentVars) => DeleteCommentAsync(id),
    onSuccess: (_res, vars) => {
      qc.invalidateQueries({ queryKey: ["comments", vars.movieId] });
    },
    onError: (error) => {
      message.error(`Failed to delete comment. ${error.message}`);
    },
  });
}