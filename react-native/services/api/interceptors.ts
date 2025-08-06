import { logout, refreshToken } from "@/redux/auth/auth-slice";
import { store } from "@/redux/store";
import { showErrorToast } from "@/utils/toast";
import { AxiosRequestConfig } from "axios";
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
        showErrorToast("Your session has expired, please log in again.");
        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    } else if (error.response.status === 403) {
      // TODO: Handle 403 Forbidden error: navigate to a forbidden page or show a message
      showErrorToast("Access denied. You do not have permission.");
      return Promise.reject(error);
    } else if (error.response.status === 400 || error.response.status === 404) {
      if (error.response?.data && error.response?.data.error?.isShow) {
        const errors: string[] = error.response?.data.error.errors || [];
        errors.forEach((err) => showErrorToast(err));
      }
      return Promise.reject(error);
    } else if (error.response.status === 429) {
      showErrorToast("Too many requests. Please try again later.");
      return Promise.reject(error);
    } else {
      showErrorToast("Something went wrong. Please try again.");
      return Promise.reject(error);
    }
  }
);
