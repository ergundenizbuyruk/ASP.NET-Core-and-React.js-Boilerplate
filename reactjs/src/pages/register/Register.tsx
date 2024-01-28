import React, { useState } from "react";
import { Checkbox } from "primereact/checkbox";
import { Button } from "primereact/button";
import { Password } from "primereact/password";
import { InputText } from "primereact/inputtext";
import { classNames } from "primereact/utils";
import { Link, useNavigate } from "react-router-dom";
import { useFormik } from "formik";
import { useTranslation } from "react-i18next";
import accountService from "../../services/account/account.service";
import { useAuth } from "../../utils/auth";
import { CreateUserDto } from "../../services/account/account.dto";
import { InputNumber } from "primereact/inputnumber";
import FormikValueIsValid from "../../utils/FormikValueIsValid";
import { useToast } from "../../utils/toast";

const RegisterPage = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const toast = useToast();
  const containerClassName = classNames(
    "surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden pt-5"
  );

  const formik = useFormik<CreateUserDto>({
    initialValues: {
      userName: "",
      email: "",
      password: "",
      firstName: "",
      lastName: "",
      phoneNumber: "",
    },
    enableReinitialize: true,
    validate: (data) => {
      const errors: any = {};

      if (!data.userName || data.userName === "") {
        errors.userName = t("UsernameIsRequired");
      }

      if (!data.email || data.email === "") {
        errors.email = t("EmailIsRequired");
      }

      if (!data.password || data.password === "") {
        errors.password = t("PasswordIsRequired");
      }

      if (!data.firstName || data.firstName === "") {
        errors.firstName = t("FirstNameIsRequired");
      }

      if (!data.lastName || data.lastName === "") {
        errors.lastName = t("LastNameIsRequired");
      }

      if (!data.phoneNumber || data.phoneNumber === "") {
        errors.phoneNumber = t("PhoneNumberIsRequired");
      }

      return errors;
    },
    onSubmit: (values) => {
      accountService.Register(values).then((res) => {
        if (res && res.result) {
          toast.show(t("RegisterSuccessful"), "success");
          navigate("/login");
        }
      });
    },
  });

  return (
    <form className={containerClassName} onSubmit={formik.handleSubmit}>
      <div className="flex flex-column align-items-center justify-content-center">
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
                {t("Register")}
              </div>
              <span className="text-600 font-medium">
                {t("RegisterToContinue")}
              </span>
            </div>

            <div>
              <label
                htmlFor="firstName"
                className="block text-900 text-xl font-medium mb-2"
              >
                {t("FirstName")}
              </label>
              <InputText
                id="firstName"
                type="text"
                placeholder={t("FirstName")}
                className={classNames("w-full md:w-30rem", {
                  "p-invalid": FormikValueIsValid(formik, "firstName"),
                })}
                value={formik.values.firstName}
                onChange={(e) =>
                  formik.setFieldValue("firstName", e.target.value)
                }
                style={{ padding: "1rem" }}
              />
              {FormikValueIsValid(formik, "firstName") && (
                <div className="p-error mt-3">{formik.errors.firstName}</div>
              )}

              <label
                htmlFor="lastName"
                className="block text-900 text-xl font-medium mb-2 mt-3"
              >
                {t("LastName")}
              </label>
              <InputText
                id="lastName"
                type="text"
                placeholder={t("LastName")}
                className={classNames("w-full md:w-30rem", {
                  "p-invalid": FormikValueIsValid(formik, "lastName"),
                })}
                value={formik.values.lastName}
                onChange={(e) =>
                  formik.setFieldValue("lastName", e.target.value)
                }
                style={{ padding: "1rem" }}
              />
              {FormikValueIsValid(formik, "lastName") && (
                <div className="p-error mt-3">{formik.errors.lastName}</div>
              )}

              <label
                htmlFor="userName"
                className="block text-900 text-xl font-medium mb-2 mt-3"
              >
                {t("UserName")}
              </label>
              <InputText
                id="userName"
                type="text"
                placeholder={t("UserName")}
                className={classNames("w-full md:w-30rem", {
                  "p-invalid": FormikValueIsValid(formik, "userName"),
                })}
                value={formik.values.userName}
                onChange={(e) =>
                  formik.setFieldValue("userName", e.target.value)
                }
                style={{ padding: "1rem" }}
              />
              {FormikValueIsValid(formik, "userName") && (
                <div className="p-error mt-3">{formik.errors.userName}</div>
              )}

              <label
                htmlFor="email"
                className="block text-900 text-xl font-medium mb-2 mt-3"
              >
                {t("Email")}
              </label>
              <InputText
                id="email"
                type="email"
                placeholder={t("EmailAddress")}
                className={classNames("w-full md:w-30rem", {
                  "p-invalid": FormikValueIsValid(formik, "email"),
                })}
                value={formik.values.email}
                onChange={(e) => formik.setFieldValue("email", e.target.value)}
                style={{ padding: "1rem" }}
              />
              {FormikValueIsValid(formik, "email") && (
                <div className="p-error mt-3">{formik.errors.email}</div>
              )}

              <label
                htmlFor="phoneNumber"
                className="block text-900 text-xl font-medium mb-2 mt-3"
              >
                {t("PhoneNumber")}
              </label>
              <InputText
                id="phoneNumber"
                type="string"
                placeholder={t("PhoneNumber")}
                className={classNames("w-full md:w-30rem", {
                  "p-invalid": FormikValueIsValid(formik, "phoneNumber"),
                })}
                value={formik.values.phoneNumber}
                onChange={(e) =>
                  formik.setFieldValue("phoneNumber", e.target.value)
                }
                style={{ padding: "1rem" }}
              />
              {FormikValueIsValid(formik, "phoneNumber") && (
                <div className="p-error mt-3">{formik.errors.phoneNumber}</div>
              )}

              <label
                htmlFor="password"
                className="block text-900 font-medium text-xl mb-2 mt-3"
              >
                {t("Password")}
              </label>
              <Password
                inputId="password"
                value={formik.values.password}
                onChange={(e) =>
                  formik.setFieldValue("password", e.target.value)
                }
                placeholder={t("Password")}
                toggleMask
                className={classNames("w-full", {
                  "p-invalid": FormikValueIsValid(formik, "password"),
                })}
                inputClassName="w-full p-3 md:w-30rem"
              ></Password>
              {FormikValueIsValid(formik, "password") && (
                <div className="p-error mt-3">{formik.errors.password}</div>
              )}

              <div className="w-full">
                <Button
                  label={t("SignUp")}
                  className="w-full text-xl mt-5"
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

export default RegisterPage;
