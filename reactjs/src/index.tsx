import React from "react";
import ReactDOM from "react-dom/client";
import "./lib/i18n";
import "primereact/resources/primereact.css";
import "primeflex/primeflex.css";
import "primeicons/primeicons.css";
import "./assets/styles/layout.scss";
import axios from "axios";
import reportWebVitals from "./reportWebVitals";
import RootLayout from "./layout/RootLayout";
import { BrowserRouter } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import AppRouter from "./lib/AppRouter";
import { AuthProvider } from "./utils/auth";
import { AxiosProvider } from "./utils/axios";
import { ToastProvider } from "./utils/toast";

axios.defaults.baseURL = process.env.REACT_APP_BASE_URL;

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      refetchOnMount: true,
      refetchOnReconnect: false,
    },
  },
});

root.render(
  <BrowserRouter>
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <ToastProvider>
          <AxiosProvider>
            <RootLayout>
              <AppRouter />
            </RootLayout>
          </AxiosProvider>
        </ToastProvider>
      </AuthProvider>
    </QueryClientProvider>
  </BrowserRouter>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
