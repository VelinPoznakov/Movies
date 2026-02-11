import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useUser } from "../Contexts/useUser";

type Props = {
  roles?: string[]
}

function ProtectedRoutes({roles}: Props){

  const {user, isLoading} = useUser();
  const location = useLocation();

  if(isLoading) return <div>Loading...</div>

  if(!user){
    return <Navigate to="/login" state={{from: location}} replace />
  }

  if(roles?.length && !roles.some((role) => user.roles.includes(role))){
    return <Navigate to="/" replace />
  }

  return <Outlet />

}

export default ProtectedRoutes;