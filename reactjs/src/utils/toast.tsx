import React, { createContext, useContext, useRef } from "react";
import { ErrorDto } from "../services/base/base.dto";
import { Toast, ToastMessage } from "primereact/toast";
import { useTranslation } from "react-i18next";
import { ConfirmDialog } from "primereact/confirmdialog";

interface ToastContextModel {
  showToast: (toasts: ToastMessage[]) => void;
  showErrorsToast: (errorDto: ErrorDto) => void;
  show: (
    message: string,
    type: "success" | "info" | "warn" | "error" | undefined
  ) => void;
}

let ToastContext = createContext<ToastContextModel>(null!);

export const useToast = () => {
  return useContext(ToastContext);
};

export const ToastProvider = ({ children }: { children: React.ReactNode }) => {
  const toast = useRef<Toast>(null);
  const { t } = useTranslation();

  const showToast = (toasts: ToastMessage[]) => {
    toast?.current?.show(toasts);
  };

  const showErrorsToast = (errorDto: ErrorDto) => {
    var toasts: ToastMessage[] = [];
    for (let error of errorDto.errors) {
      toasts.push({
        severity: "error",
        summary: t("Sorry"),
        detail: <div>{error}</div>,
        life: 5000,
      });
    }

    toast?.current?.show(toasts);
  };

  const show = (
    message: string,
    type: "success" | "info" | "warn" | "error" | undefined
  ) => {
    var summary = "";
    if (type === "success") {
      summary = t("Successful");
    } else if (type === "error") {
      summary = t("Sorry");
    } else if (type === "warn") {
      summary = t("Warning");
    } else if (type === "info") {
      summary = t("Information");
    }

    toast?.current?.show({
      severity: type,
      summary: summary,
      detail: message,
      closable: false,
      className: "custom-toast",
      life: 5000,
    });
  };

  const value = { showToast, showErrorsToast, show };

  return (
    <ToastContext.Provider value={value}>
      <Toast ref={toast} position="top-right" />
      <ConfirmDialog />
      {children}
    </ToastContext.Provider>
  );
};
