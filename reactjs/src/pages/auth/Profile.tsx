import React, { useEffect, useState } from "react";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { classNames } from "primereact/utils";
import { useNavigate } from "react-router-dom";
import { useFormik } from "formik";
import { useTranslation } from "react-i18next";
import accountService from "../../services/account/account.service";
import { useAuth } from "../../utils/auth";
import { UpdateProfileDto } from "../../services/account/account.dto";
import FormikValueIsValid from "../../utils/FormikValueIsValid";
import { useToast } from "../../utils/toast";

const ProfilePage = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const toast = useToast();
  const user = useAuth();

  useEffect(() => {
    accountService.GetUserInformation().then((res) => {
      if (res && res.result?.data) {
        formik.setFieldValue("firstName", res.result.data.firstName);
        formik.setFieldValue("lastName", res.result.data.lastName);
        formik.setFieldValue("phoneNumber", res.result.data.phoneNumber);
      }
    });
  }, [user]);

  const containerClassName = classNames(
    "surface-ground flex align-items-center justify-content-center pt-5"
  );

  const formik = useFormik<UpdateProfileDto>({
    initialValues: {
      firstName: "",
      lastName: "",
      phoneNumber: "",
    },
    enableReinitialize: true,
    validate: (data) => {
      const errors: any = {};

      if (!data.firstName) {
        errors.firstName = t("FirstNameIsRequired");
      }

      if (!data.lastName) {
        errors.lastName = t("LastNameIsRequired");
      }

      if (!data.phoneNumber) {
        errors.phoneNumber = t("PhoneNumberIsRequired");
      }

      return errors;
    },
    onSubmit: (values) => {
      accountService.UpdateProfile(values).then((res) => {
        if (!res?.result?.error) {
          toast.show(t("UpdateProfileSuccessful"), "success");
          navigate("/app/homepage");
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
                {t("UpdateProfile")}
              </div>
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

              <div className="w-full">
                <Button
                  label={t("Update")}
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

export default ProfilePage;
