import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getUserProfile, loginUser, logoutUser, registerUser } from "../../api/fetching/authService";

export function useGetUser(){
  return useQuery({
    queryKey: ["user"],
    queryFn: getUserProfile,
    staleTime: 5 * 60 * 1000,
    retry: false,
  });
}

export function useLogin(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: loginUser,
    onSuccess: (data) => {
      qc.invalidateQueries({ queryKey: ["user"] });
      localStorage.setItem("token", data.token);
    }
  });
}

export function useRegister(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: registerUser,
    onSuccess: (data) => {
      qc.invalidateQueries({ queryKey: ["user"] });
      localStorage.setItem("token", data.token);
    }
  });
}

export function useLogout(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: logoutUser,
    onSuccess: () => {
      qc.clear();
      localStorage.clear();
    }
  });
}