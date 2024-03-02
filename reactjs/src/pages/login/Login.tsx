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
import authService from "../../services/auth/auth.service";
import { useAuth } from "../../utils/auth";

const Login = () => {
  const [checked, setChecked] = useState(false);
  const { t } = useTranslation();
  const auth = useAuth();

  const navigate = useNavigate();
  const containerClassName = classNames(
    "surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden"
  );

  const formik = useFormik<LoginDto>({
    initialValues: {
      email: "",
      password: "",
    },
    enableReinitialize: true,
    validate: (data) => {
      const errors: any = {};

      if (!data.email) {
        errors.email = t("EmailIsRequired");
      }

      if (!data.password) {
        errors.password = t("PasswordIsRequired");
      }

      return errors;
    },
    onSubmit: (values) => {
      authService.CreateToken(values).then((res) => {
        if (res.result && !res.result.error) {
          auth.setToken(res.result.data, checked);
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
                {t("Welcome")}
              </div>
              <span className="text-600 font-medium">
                {t("SignInToContinue")}
              </span>
            </div>

            <div>
              <label
                htmlFor="email"
                className="block text-900 text-xl font-medium mb-2"
              >
                {t("Email")}
              </label>
              <InputText
                id="email"
                type="text"
                placeholder={t("EmailAddress")}
                className="w-full md:w-30rem"
                value={formik.values.email}
                onChange={(e) => formik.setFieldValue("email", e.target.value)}
                style={{ padding: "1rem" }}
              />
              {formik.errors.email && (
                <div className="p-error mt-3">{formik.errors.email}</div>
              )}

              <label
                htmlFor="password"
                className="block text-900 font-medium text-xl mb-2 mt-5"
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
                className="w-full"
                inputClassName="w-full p-3 md:w-30rem"
                feedback={false}
              ></Password>
              {formik.errors.password && (
                <div className="p-error mt-3">{formik.errors.password}</div>
              )}

              <div className="flex align-items-center justify-content-between mb-5 gap-5 mt-5">
                <div className="flex align-items-center">
                  <Checkbox
                    inputId="rememberme"
                    checked={checked}
                    onChange={(e) => setChecked(e.checked ?? false)}
                    className="mr-2"
                  ></Checkbox>
                  <label htmlFor="rememberme">{t("RememberMe")}</label>
                </div>
                <Link
                  to="/forgot-password"
                  className="font-medium no-underline ml-2 text-right cursor-pointer"
                  style={{ color: "var(--primary-color)" }}
                >
                  {t("ForgotPassword")}
                </Link>
              </div>
              <Button
                label={t("Login")}
                className="w-full p-3 text-xl"
                type="submit"
              ></Button>
            </div>
          </div>
        </div>
      </div>
    </form>
  );
};

export default Login;
