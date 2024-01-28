import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useAuth } from "./auth";
import { useNavigate } from "react-router-dom";
import { ErrorDto, HttpResponse } from "../services/base/base.dto";
import axios from "axios";
import { ProgressSpinner } from "primereact/progressspinner";
import { BlockUI } from "primereact/blockui";
import authService from "../services/auth/auth.service";
import { AccessTokenDto, RefreshTokenDto } from "../services/auth/auth.dto";
import { useToast } from "./toast";

let AxiosContext = React.createContext(null);
export function useAxiosContext() {
  return React.useContext(AxiosContext);
}

export function AxiosProvider({ children }: { children: React.ReactNode }) {
  const toast = useToast();
  const { t } = useTranslation();
  const auth = useAuth();
  let navigate = useNavigate();
  const [blocked, setBlocked] = useState<boolean>(false);
  const [run, setRun] = useState<boolean>(false);

  useEffect(() => {
    if (!run) {
      axios.interceptors.request.use(
        (config) => {
          if (config.blockUI) {
            setBlocked(true);
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
          if (error.config.blockUI) {
            setBlocked(false);
          }
          if (error.response.status === 401) {
            try {
              var user = auth.getUser();
              const data: RefreshTokenDto = {
                token: user.refreshToken,
              };
              const response = await authService.CreateTokenByRefreshToken(
                data
              );
              if (response.result) {
                auth.setToken(response.result.data, true);
                error.config.headers["Authorization"] =
                  "Bearer " + response.result.data.accessToken;
                return axios.request({
                  ...error.config,
                  cache: false,
                  overrideError: true,
                });
              }
              return Promise.reject(error);
            } catch (error) {
              auth.removeUserFromStorage();
              navigate("/login", { replace: true });
              return Promise.reject(error);
            }
          } else if (error.response.status === 403) {
            auth.removeUserFromStorage();
            navigate("/login", { replace: true });
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

      setRun(true);
    }
  }, []);

  const pageLoader = () => {
    return (
      <div className="flex flex-column align-items-center justify-content-center w-screen h-screen">
        <span className="font-bold text-white text-6xl">{t("Loading")}</span>
        <ProgressSpinner />
      </div>
    );
  };

  return (
    <AxiosContext.Provider value={null}>
      <BlockUI blocked={blocked} fullScreen template={pageLoader}>
        {children}
      </BlockUI>
    </AxiosContext.Provider>
  );
}
