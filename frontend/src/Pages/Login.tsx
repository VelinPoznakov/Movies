import { LockOutlined, UserOutlined } from "@ant-design/icons";
import { Button, Form, Input, message } from "antd";
import { useLogin } from "../Queries/Auth/authHook";
import { Link } from "react-router";


type Values = {
  username: string;
  password: string;
}

function Login(){
  const [form] = Form.useForm();
  const {mutateAsync: login, isPending} = useLogin();

  const onFinish = async (value: Values) => {
    try{
      await login(value);
    }catch{
      message.error("Login failed. Please check your credentials and try again.");
    }
  }
  
  return (
    <div style={{ maxWidth: 360, margin: "0 auto" }}>
      <Form form={form} layout="vertical" onFinish={onFinish}>
        <Form.Item
          label="Username"
          name="username"
          rules={[
            { required: true, message: "This field is required" },
            { min: 3, message: "Username must be at least 3 characters long" },
            { max: 20, message: "Username must be less than 20 characters long" },
            {
              pattern:
                /^[a-zA-Z0-9]+([a-zA-Z0-9](_|-| )[a-zA-Z0-9])*[a-zA-Z0-9]+$/,
              message: "Username must only contain letters, numbers and spaces.",
            },
          ]}
          hasFeedback
        >
          <Input placeholder="username" prefix={<UserOutlined />} />
        </Form.Item>

        <Form.Item
          label="Password"
          name="password"
          rules={[
            { required: true, message: "This field is required" },
            {
              pattern:
                /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/,
              message:
                "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number and one special character.",
            },
          ]}
          hasFeedback
        >
          <Input.Password placeholder="password" prefix={<LockOutlined />} />
        </Form.Item>

        <Form.Item>
          <Link to="/register">
            Don't have an account? Register here.
          </Link>
        </Form.Item>

        <Button type="primary" htmlType="submit" block loading={isPending}>
          Log In
        </Button>
      </Form>
    </div>
  );
}

export default Login;