import React from "react";
import { Button } from "primereact/button";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

const AccessDeniedPage = () => {
  const navigate = useNavigate();
  const { t } = useTranslation();

  return (
    <div className="surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden">
      <div className="flex flex-column align-items-center justify-content-center">
        <img
          src="/images/logo-orange.svg"
          alt="Sakai logo"
          className="mb-5 w-6rem flex-shrink-0"
        />
        <div
          style={{
            borderRadius: "56px",
            padding: "0.3rem",
            background:
              "linear-gradient(180deg, rgba(247, 149, 48, 0.4) 10%, rgba(247, 149, 48, 0) 30%)",
          }}
        >
          <div
            className="w-full surface-card py-8 px-5 sm:px-8 flex flex-column align-items-center"
            style={{ borderRadius: "53px" }}
          >
            <div
              className="flex justify-content-center align-items-center bg-pink-500 border-circle"
              style={{ height: "3.2rem", width: "3.2rem" }}
            >
              <i className="pi pi-fw pi-exclamation-circle text-2xl text-white"></i>
            </div>
            <h1 className="text-900 font-bold text-5xl mb-2">
              {t("AccessDenied")}
            </h1>
            <div className="text-600 mb-5">{t("AccessDeniedMessage")}</div>
            <img
              src="/images/asset-access.svg"
              alt="Error"
              className="mb-5"
              width="80%"
            />
            <Button
              icon="pi pi-arrow-left"
              label={t("GoToDashboard")}
              text
              onClick={() => navigate("/")}
            />
          </div>
        </div>
      </div>
    </div>
  );
};

export default AccessDeniedPage;
