import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getSession, getUserProfile, loginUser, logoutUser, registerUser } from "../../api/fetching/authService";
import { setAccessToken } from "../../accessToken";
import { useNavigate } from "react-router";
import type { LoginRequest, RegisterRequest } from "../../types/auth";

export function useGetUser() {
  return useQuery({
    queryKey: ["user"],
    queryFn: async () => {
      const session = await getSession();

      if (!session?.user) {
        setAccessToken(null);
        return null;
      }

      if (session.token) {
        setAccessToken(session.token);
      }

      return session.user;
    },
    staleTime: 5 * 60 * 1000,
    retry: false,
  });
}

export function useLogin(){
  const qc = useQueryClient();
  const navigate = useNavigate();

  return useMutation({
    mutationFn: ({username, password}: LoginRequest) => loginUser({username, password}),
    onSuccess: (data) => {
      setAccessToken(data.token);
      qc.invalidateQueries({ queryKey: ["user"] });
      navigate("/", { replace: true });
    }
  });
}

export function useRegister(){
  const qc = useQueryClient();
  const navigate = useNavigate();

  return useMutation({
    mutationFn: ({username, email, password}: RegisterRequest) => registerUser({username, email, password}),
    onSuccess: (data) => {
      setAccessToken(data.token);
      qc.invalidateQueries({ queryKey: ["user"] });
      navigate("/", { replace: true });
    }
  });
}

export function useLogout() {
  const qc = useQueryClient();
  const navigate = useNavigate();

  return useMutation({
    mutationFn: logoutUser,
    onSuccess: () => {
      setAccessToken(null);

      qc.setQueryData(["user"], null);

      qc.removeQueries({ queryKey: ["user"] });

      navigate("/", { replace: true });
    },
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