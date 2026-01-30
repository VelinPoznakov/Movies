import { createContext, type ReactNode} from "react";
import { useGetUser } from "../Queries/Auth/authHook";

export const AuthContext = createContext<any>(null);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const { data: user, isLoading } = useGetUser();

  return (
    <AuthContext.Provider value={{ user, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
};
