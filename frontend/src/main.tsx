import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { RouterProvider } from 'react-router';
import { router } from './Routes/router';
import { UserProvider } from './Contexts/authContext';
import "antd/dist/reset.css";
import "./index.css";
import "@ant-design/icons"


const queryClient = new QueryClient();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <UserProvider>
        <RouterProvider router={router} />
      </UserProvider>
    </QueryClientProvider>
  </StrictMode>,
)
