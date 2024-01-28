import React, { useTransition } from "react";
import { useTranslation } from "react-i18next";

function Homepage() {
  const {t} = useTranslation();
  return <>
    Home Page <div>{t("HelloWorld")}</div>
  </>;
}

export default Homepage;
