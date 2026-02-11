import { Menu } from "antd";
import { Link, useLocation } from "react-router-dom";
import { useUser } from "../Contexts/useUser";
import { useLogout } from "../Queries/Auth/authHook";

function NavbarComponent() {
  const location = useLocation();
  const {user} = useUser();
  const {mutate: logout} = useLogout();

  const leftSideMenu = [
    { key: "/", label: <Link to="/">Home</Link> },
    { key: "/about", label: <Link to="/about">About</Link> },
    user ? { key: "/create-movie", label: <Link to="/create-movie">Create Movie</Link> }: null,

  ]

  const rightSideMenu = 
    !user ? [
      {key: "/login", label: <Link to="/login">Login</Link>},
      {key: "/register", label: <Link to="/register">Register</Link>},
    ] : [
      {key: "/profile", label: <Link to="/profile">My Profile</Link>},
      {key: "Logout", label: "Logout"},
      user.roles.includes("Admin") || user.roles.includes("SuperAdmin") ?
        {key: "/admin-panel", label: <Link to="/admin-panel">Admin Panel</Link>} : null,
    ]


  return (
    <div 
        style={{
          display: "flex",
          alignItems: "center",
          width: "100%",
          overflowX: "auto",
          overflowY: "hidden",
          }}
      >
      <Menu
        theme="dark"
        mode="horizontal"
        selectedKeys={[location.pathname]}
        items={leftSideMenu}
        disabledOverflow
        style={{ flex: "1 1 auto", minWidth: 0, borderBottom: "none" }}
      />

      <Menu
        theme="dark"
        mode="horizontal"
        selectedKeys={[location.pathname]}
        items={rightSideMenu}
        disabledOverflow
        style={{ borderBottom: "none", flexShrink: 0 }}
        onClick={({key}) => {
          if(key === "Logout"){
            logout();
          }
        }}
      />
    </div>
  );
}

export default NavbarComponent;
