import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { CreateGenreAsync, DeleteGenreAsync, GetAllGenresAsync, GetGenreByIdAsync, UpdateGenreAsync } from "../../api/fetching/genreService";
import type { CreateGenreRequest, UpdateGenreRequest } from "../../types/genreTypes";
import { message } from "antd";

export function useGetAllGenres(){

  return useQuery({
    queryKey: ["genres"],
    queryFn: GetAllGenresAsync,
    staleTime: 5 * 60 * 1000,
  })
}

export function useGetGenre(id: number){

  return useQuery({
    queryKey: ["genre", id],
    queryFn: () => GetGenreByIdAsync(id),
  })
}

export function useCreateGenreQuery(data: CreateGenreRequest){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: () => CreateGenreAsync(data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["genres"] });
    },
    onError: (error) => {
      message.error(`Failed to create genre ${error.message}`);
    },
  })
}

type UpdateGenreVariables = {
  id: number;
  data: UpdateGenreRequest;
}

export function useUpdateGenre(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: ({id, data}: UpdateGenreVariables) => UpdateGenreAsync(id, data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["genres"] });
    },
    onError: (error) => {
      message.error(`Failed to update genre ${error.message}`);
    },
  })
}

export function useDeleteGenre(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => DeleteGenreAsync(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["genres"] });
    },
    onError: (error) => {
      message.error(`Failed to delete genre ${error.message}`);
    },
  })
}