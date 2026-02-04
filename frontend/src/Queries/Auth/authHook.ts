import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getSession, getUserProfile, loginUser, logoutUser, registerUser } from "../../api/fetching/authService";
import { setAccessToken } from "../../accessToken";

export function useGetUser(){
  return useQuery({
    queryKey: ["user"],
    queryFn: getSession,
    staleTime: 5 * 60 * 1000,
    retry: false,
  });
}

export function useLogin(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: loginUser,
    onSuccess: (data) => {
      setAccessToken(data.token);
      qc.invalidateQueries({ queryKey: ["user"] });
    }
  });
}

export function useRegister(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: registerUser,
    onSuccess: (data) => {
      setAccessToken(data.token);
      qc.invalidateQueries({ queryKey: ["user"] });
    }
  });
}

export function useLogout(){
  const qc = useQueryClient();

  return useMutation({
    mutationFn: logoutUser,
    onSuccess: () => {
      setAccessToken(null);
      qc.clear();
    }
  });
}

export function useProfile(){

  return useQuery({
    queryFn: getUserProfile,
    queryKey: ["profile"],
    staleTime: 5 * 60 * 1000,
    retry: false,
  })
}