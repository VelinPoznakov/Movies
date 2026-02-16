import { Alert, Button, Form, Input } from "antd";
import { useRegister } from "../Queries/Auth/authHook";
import UserOutlined from "@ant-design/icons/lib/icons/UserOutlined";
import { LockOutlined, MailOutlined } from "@ant-design/icons";
import { Link } from "react-router-dom";
import axios from "axios";
import { useState } from "react";

type RegisterProps ={
  username: string;
  email: string;
  password: string;
}

function RegisterPage() {
  const [form] = Form.useForm();
  const [apiError, setApiError] = useState<string | null>(null);

  const { mutateAsync: register, isPending } = useRegister();

  const onFinish = async (value: RegisterProps) => {
    setApiError(null); // clear previous error
    try {
      await register(value);
    } catch (err) {
      // read server message and show it in the form
      if (axios.isAxiosError(err)) {
        const msg = (err.response?.data)?.message;
        if (msg) setApiError(msg);
        else setApiError("Registration failed.");
      } else {
        setApiError("Registration failed.");
      }
    }
  };


  return (
    <div style={{ maxWidth: 360, margin: "0 auto" }}>
      {apiError && (
        <Alert
          type="error"
          showIcon
          title={apiError}
          style={{ marginBottom: 12 }}
        />
      )}


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
          label="email"
          name="email"
          rules={[
            { required: true, message: "This field is required" },
            {
              pattern:
                /^((?!\.)[\w-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$/gim,
              message: "Please enter a valid email address.",
            }
          ]}
          hasFeedback
        >
          <Input placeholder="email" prefix={<MailOutlined /> } />

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
          <Link to="/login">
            Already have an account? Log in here.
          </Link>
        </Form.Item>

        <Button type="primary" htmlType="submit" block loading={isPending}>
          Register
        </Button>
      </Form>
    </div>
  );
}

export default RegisterPage;