import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useAuth } from "./auth";
import { useNavigate } from "react-router-dom";
import axios, { AxiosRequestConfig } from "axios";
import { ProgressSpinner } from "primereact/progressspinner";
import { BlockUI } from "primereact/blockui";
import authService from "../services/auth/auth.service";
import { RefreshTokenDto } from "../services/auth/auth.dto";
import { useToast } from "./toast";

let AxiosContext = React.createContext(null);
export function useAxiosContext() {
  return React.useContext(AxiosContext);
}

// Define the structure of a retry queue item
interface RetryQueueItem {
  resolve: (value?: any) => void;
  reject: (error?: any) => void;
  config: AxiosRequestConfig;
}

export function AxiosProvider({ children }: { children: React.ReactNode }) {
  const toast = useToast();
  const { t } = useTranslation();
  const auth = useAuth();
  let navigate = useNavigate();
  const [blocked, setBlocked] = useState<boolean>(false);
  const [isAxiosInterceptorAdded, setIsAxiosInterceptorAdded] =
    useState<boolean>(false);

  // Create a list to hold the request queue
  const refreshAndRetryQueue: RetryQueueItem[] = [];

  // Flag to prevent multiple token refresh requests
  let isRefreshing = false;

  if (!isAxiosInterceptorAdded) {
    axios.interceptors.request.use(
      (config) => {
        if (config.blockUI === undefined) {
          if (
            config.method === "post" ||
            config.method === "put" ||
            config.method === "delete"
          ) {
            setBlocked(true);
            config.blockUI = true;
          }
        } else {
          setBlocked(config.blockUI);
        }
        return config;
      },
      (error) => {
        return Promise.reject(error);
      }
    );

    axios.interceptors.response.use(
      (response) => {
        if (response.config.blockUI) {
          setBlocked(false);
        }
        return response;
      },
      async (error) => {
        const originalRequest: AxiosRequestConfig = error.config;
        if (error.config.blockUI) {
          setBlocked(false);
        }
        if (error.response.status === 401) {
          if (!isRefreshing) {
            isRefreshing = true;
            try {
              var user = auth.getUser();
              const data: RefreshTokenDto = {
                token: user.refreshToken,
              };
              const newToken = await authService.CreateTokenByRefreshToken(
                data,
                {
                  blockUI: false,
                }
              );

              if (newToken.result && newToken.result.data) {
                auth.setToken(newToken.result.data, true);
                error.config.headers["Authorization"] =
                  "Bearer " + newToken.result.data.accessToken;

                // Retry all requests in the queue with the new token
                refreshAndRetryQueue.forEach(({ config, resolve, reject }) => {
                  if (newToken.result && config.headers) {
                    // override the bearer token
                    config.headers["Authorization"] =
                      "Bearer " + newToken.result.data.accessToken;
                    axios
                      .request(config)
                      .then((response) => resolve(response))
                      .catch((err) => reject(err));
                  }
                });

                // Clear the queue
                refreshAndRetryQueue.length = 0;

                // Retry the original request
                return axios(originalRequest);
              } else {
                auth.removeUserFromStorage();
                navigate("/login", { replace: true });
                return Promise.reject(error);
              }
            } catch (error) {
              auth.removeUserFromStorage();
              navigate("/login", { replace: true });
              return Promise.reject(error);
            } finally {
              isRefreshing = false;
            }
          }

          // Add the original request to the queue
          return new Promise<void>((resolve, reject) => {
            refreshAndRetryQueue.push({
              config: originalRequest,
              resolve,
              reject,
            });
          });
        } else if (error.response.status === 403) {
          auth.removeUserFromStorage();
          navigate("/access-denied", { replace: true });
          return Promise.reject(error);
        } else if (
          error.response.status === 400 ||
          error.response.status === 404
        ) {
          if (error.response?.data && error.response?.data.error.isShow) {
            toast.showErrorsToast(error.response?.data.error);
          }
          return Promise.reject(error);
        } else {
          toast.show(t("SomethingWentWrong"), "error");
          return Promise.reject(error);
        }
      }
    );

    setIsAxiosInterceptorAdded(true);
  }

  const pageLoader = () => {
    return (
      <div className="flex flex-column align-items-center justify-content-center w-screen h-screen">
        <ProgressSpinner />
      </div>
    );
  };

  return (
    <>
      <AxiosContext.Provider value={null}>
        <BlockUI blocked={blocked} fullScreen template={pageLoader}>
          {children}
        </BlockUI>
      </AxiosContext.Provider>
    </>
  );
}
