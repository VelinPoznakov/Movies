import { useGetUser } from "../Queries/Auth/authHook";
import { UserContext } from "./useUser";



export function UserProvider({children}: {children: React.ReactNode}){
  const {data, isLoading} = useGetUser();

  return (
    <UserContext.Provider value={{user: data ?? null, isLoading}}>
      {children}
    </UserContext.Provider>
  )
}



