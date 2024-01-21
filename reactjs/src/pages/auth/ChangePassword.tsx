import React, { useState } from "react";
import { Checkbox } from "primereact/checkbox";
import { Button } from "primereact/button";
import { Password } from "primereact/password";
import { InputText } from "primereact/inputtext";
import { classNames } from "primereact/utils";
import { Link, useNavigate } from "react-router-dom";
import { useFormik } from "formik";
import { useTranslation } from "react-i18next";
import { LoginDto } from "../../services/auth/auth.dto";
import accountService from "../../services/account/account.service";
import { useAuth } from "../../utils/auth";
import { ChangePasswordDto } from "../../services/account/account.dto";
import { useToast } from "../../utils/toast";
import FormikValueIsValid from "../../utils/FormikValueIsValid";

const ChangePasswordPage = () => {
  const { t } = useTranslation();
  const toast = useToast();

  const navigate = useNavigate();
  const containerClassName = classNames(
    "surface-ground flex align-items-center justify-content-center overflow-hidden"
  );

  const formik = useFormik<ChangePasswordDto>({
    initialValues: {
      currentPassword: "",
      newPassword: "",
    },
    enableReinitialize: true,
    validate: (data) => {
      const errors: any = {};

      if (data.currentPassword === undefined || data.currentPassword === "") {
        errors.currentPassword = t("PasswordIsRequired");
      }

      if (data.newPassword === undefined || data.newPassword === "") {
        errors.newPassword = t("PasswordIsRequired");
      }

      return errors;
    },
    onSubmit: (values) => {
      accountService.ChangePassword(values).then((res) => {
        if (!res.error) {
          toast.show(t("ChangePasswordMessage"), "success");
          navigate("/app/homepage", { replace: true });
        }
      });
    },
  });

  return (
    <form className={containerClassName} onSubmit={formik.handleSubmit}>
      <div className="flex flex-column align-items-center justify-content-center">
        <img
          src="/images/logo-dark.svg"
          alt="Sakai logo"
          className="mb-5 w-6rem flex-shrink-0"
        />
        <div
          style={{
            borderRadius: "56px",
            padding: "0.3rem",
            background:
              "linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)",
          }}
        >
          <div
            className="w-full surface-card py-8 px-5 sm:px-8"
            style={{ borderRadius: "53px" }}
          >
            <div className="text-center mb-5">
              <img
                src="/images/logo-dark.svg"
                alt="Image"
                height="50"
                className="mb-3"
              />
              <div className="text-900 text-3xl font-medium mb-3">
                {t("PasswordChange")}
              </div>
            </div>

            <div>
              <label
                htmlFor="currentPassword"
                className="block text-900 text-xl font-medium mb-2"
              >
                {t("CurrentPassword")}
              </label>
              <InputText
                id="currentPassword"
                type="password"
                placeholder={t("EmailAddress")}
                className={classNames("w-full md:w-30rem", {
                  "p-invalid": FormikValueIsValid(formik, "currentPassword"),
                })}
                value={formik.values.currentPassword}
                onChange={(e) =>
                  formik.setFieldValue("currentPassword", e.target.value)
                }
                style={{ padding: "1rem" }}
              />
              {FormikValueIsValid(formik, "currentPassword") && (
                <div className="p-error mt-2">
                  {formik.errors.currentPassword}
                </div>
              )}

              <label
                htmlFor="newPassword"
                className="block text-900 text-xl font-medium mb-2 mt-3"
              >
                {t("NewPassword")}
              </label>
              <InputText
                id="newPassword"
                type="password"
                placeholder={t("EmailAddress")}
                className={classNames("w-full md:w-30rem", {
                  "p-invalid": FormikValueIsValid(formik, "newPassword"),
                })}
                value={formik.values.newPassword}
                onChange={(e) =>
                  formik.setFieldValue("newPassword", e.target.value)
                }
                style={{ padding: "1rem" }}
              />
              {FormikValueIsValid(formik, "newPassword") && (
                <div className="p-error mt-2">{formik.errors.newPassword}</div>
              )}

              <div className="w-full mt-5">
                <Button
                  label={t("ChangePassword")}
                  className="w-full p-3 text-xl"
                  type="submit"
                ></Button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </form>
  );
};

export default ChangePasswordPage;
