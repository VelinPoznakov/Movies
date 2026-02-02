import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { CreateDirectorAsync, DeleteDirectorAsync, GetAllDirectorsAsync, GetDirectorByIdAsync, UpdateDirectorAsync } from "../../api/fetching/directorsService";
import type { CreateDirectorRequest, UpdateDirectorRequest } from "../../types/directorsTypes";
import { message } from "antd";

export function useGetAllDirectors(){

  return useQuery({
    queryKey: ["directors"],
    queryFn: GetAllDirectorsAsync,
    staleTime: 5 * 60 * 1000,
  })
}

export function useGetDirectorById(id: number){

  return useQuery({
    queryKey: ["director", id],
    queryFn: () => GetDirectorByIdAsync(id),
  })
}

export function useCreateDirector(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateDirectorRequest) => CreateDirectorAsync(data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["directors"] });
    },
    onError: (error) => {
      message.error(`Failed to create director. ${error.message}`);
    }
  })
}

type UpdateDirectorData = {
  id: number;
  data: UpdateDirectorRequest;
}

export function useUpdateDirector(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: ({id, data}: UpdateDirectorData) => UpdateDirectorAsync(id, data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["directors"] });
    },
    onError: (error) => {
      message.error(`Failed to update director. ${error.message}`);
    }
  })
}

export function useDeleteDirector(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => DeleteDirectorAsync(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["directors"] });
    },
    onError: (error) => {
      message.error(`Failed to delete director. ${error.message}`);
    }
  })
}