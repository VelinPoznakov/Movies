import { createContext, useContext } from "react";
import type { UserProfile } from "../types/auth";

type UserContextValue = {
  user: UserProfile | null;
  isLoading: boolean;
}

export const UserContext = createContext<UserContextValue | null>(null);

export function useUser(){
  const ctx = useContext(UserContext);
  if(!ctx) throw new Error("useUser must be used within a UserProvider");
  return ctx;
}