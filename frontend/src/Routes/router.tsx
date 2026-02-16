import { createBrowserRouter } from "react-router-dom";
import AppLayout from "../AppLayout";
import HomePage from "../Pages/HomePage";
import ProtectedRoutes from "./ProtectedRoutes";
import Login from "../Pages/Login";
import RegisterPage from "../Pages/RegisterPage";

export const router = createBrowserRouter([

  {  
    path: "/",
    element: <AppLayout />,
    children: [
      {
        path: "/",
        element: <HomePage />
      },
      {
        path: "about",
        element: <div>about page</div>
      },
      {
        path: "contact",
        element: <div>contact page</div>
      },
      {
        path: "login",
        element: <Login />
      },
      {
        path: "register",
        element: <RegisterPage />
      },
      {
        element: <ProtectedRoutes />,
        children: [
          {
            path:"profile",
            element:<div>profile page</div>
          },
          {
            path: "create-movie",
            element: <div>create movie page</div>
          },
        ]
      },
      {
        element: <ProtectedRoutes roles={["Admin", "SuperAdmin"]} />,
        children: [
          {
            path: "admin-panel",
            element: <div>admin panel page</div>
          },
        ]
      },
    ]
  }
])