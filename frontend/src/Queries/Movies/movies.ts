import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { CreateMovieAsync, DeleteMovieAsync, GetAllMoviesAsync, GetMovieByIdAsync, UpdateMovieAsync } from "../../api/fetching/moviesService";
import type { CreateMovieRequest, UpdateMovieRequest } from "../../types/moviesTypes";
import { message } from "antd";

export function useMovies(){

  return useQuery({
    queryKey: ["movies"],
    queryFn: GetAllMoviesAsync,
    staleTime: 1000 * 60 * 5,
  })
}

export function useMovieById(id: number){
  return useQuery({
    queryKey: ["movie", id],
    queryFn: () => GetMovieByIdAsync(id),
  })
}

export function useCreateMovie(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateMovieRequest) => CreateMovieAsync(data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["movies"] });
    }
  })
}

type UpdateMovieVariable = {
  id: number;
  data: UpdateMovieRequest;
}

export function useUpdateMovie(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn:({id, data}: UpdateMovieVariable) => UpdateMovieAsync(id, data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["movies"] });
    },
    onError: (error) => {
      message.error(error.message);
    }
  });
}

export function useDeleteMovie(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => DeleteMovieAsync(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["movies"] });
    },
    onError: (error) => {
      message.error(error.message);
    }
  })
}