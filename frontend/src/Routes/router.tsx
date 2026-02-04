import { createBrowserRouter } from "react-router-dom";
import AppLayout from "../AppLayout";
import HomePage from "../Pages/HomePage";

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
        path:"profile",
        element:<div>profile page</div>
      },
      {
        path: "login",
        element: <div>login page</div>
      },
      {
        path: "register",
        element: <div>register page</div>
      },
      {
        path: "create-movie",
        element: <div>create movie page</div>
      }
    ]
  }
])