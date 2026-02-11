import { Outlet } from "react-router";
import NavbarComponent from "./Components/NavbarComponent";
import { Layout } from "antd";
import { Content, Footer } from "antd/es/layout/layout";

function AppLayout() {

  return(
    <Layout style={{ minHeight: "100vh" }}>
      <NavbarComponent />

      <Content style={{ flex: 1, padding: 24 }}>
        <Outlet />
      </Content>

      <Footer style={{ textAlign: "center" }}>© Movies</Footer>
    </Layout>
  );
}

export default AppLayout;