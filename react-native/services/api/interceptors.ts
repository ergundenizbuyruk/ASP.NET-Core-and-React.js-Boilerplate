import { logout, refreshToken } from "@/redux/auth/auth-slice";
import { store } from "@/redux/store";
import { AxiosRequestConfig } from "axios";
import { Alert } from "react-native";
import apiClient from "./axios-instance";

interface RetryQueueItem {
  resolve: (value?: any) => void;
  reject: (error?: any) => void;
  config: AxiosRequestConfig;
}

let isRefreshing = false;
const refreshAndRetryQueue: RetryQueueItem[] = [];

const processQueue = (error: any, token: string | null = null) => {
  refreshAndRetryQueue.forEach(({ resolve, reject, config }) => {
    if (token) {
      config.headers = config.headers || {};
      config.headers["Authorization"] = "Bearer " + token;
      resolve(apiClient(config));
    } else {
      reject(error);
    }
  });
  refreshAndRetryQueue.length = 0;
};

apiClient.interceptors.request.use(
  (config) => {
    const token = store.getState().auth.accessToken;
    if (token && config.headers) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          refreshAndRetryQueue.push({
            resolve,
            reject,
            config: originalRequest,
          });
        });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const result = await store.dispatch(refreshToken()).unwrap();

        const newAccessToken = result.accessToken;

        processQueue(null, newAccessToken);

        originalRequest.headers["Authorization"] = "Bearer " + newAccessToken;
        return apiClient(originalRequest);
      } catch (refreshError) {
        processQueue(refreshError, null);
        store.dispatch(logout());
        Alert.alert("Oturum Süresi Doldu", "Lütfen tekrar giriş yapın.");
        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);
