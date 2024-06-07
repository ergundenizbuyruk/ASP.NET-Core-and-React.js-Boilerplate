import { classNames } from "primereact/utils";
import React, {
  forwardRef,
  useContext,
  useImperativeHandle,
  useRef,
  useState,
} from "react";
import { AppTopbarRef } from "../types/types";
import { LayoutContext } from "./context/layoutcontext";
import i18n from "../lib/i18n";
import { locale } from "primereact/api";
import axios from "axios";
import { Menu } from "primereact/menu";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../utils/auth";

const AppTopbar = forwardRef<AppTopbarRef>((props, ref) => {
  const { layoutConfig, layoutState, onMenuToggle, showProfileSidebar } =
    useContext(LayoutContext);
  const menubuttonRef = useRef(null);
  const topbarmenuRef = useRef(null);
  const topbarmenubuttonRef = useRef(null);
  const [selectedLang, setSelectedLang] = useState(i18n.language);
  const languagesMenu = useRef<Menu>(null);
  const profileMenu = useRef<Menu>(null);
  const { t } = useTranslation();
  const navigate = useNavigate();
  const auth = useAuth();

  useImperativeHandle(ref, () => ({
    menubutton: menubuttonRef.current,
    topbarmenu: topbarmenuRef.current,
    topbarmenubutton: topbarmenubuttonRef.current,
  }));

  let languages = [
    {
      label: "Türkçe",
      command: () => {
        i18n.changeLanguage("tr");
        locale("tr");
        axios.defaults.headers.common["Accept-Language"] = "tr-TR";
        setSelectedLang("tr");
      },
    },
    {
      label: "English(US)",
      command: () => {
        i18n.changeLanguage("en");
        locale("en");
        axios.defaults.headers.common["Accept-Language"] = "en-US";
        setSelectedLang("en");
      },
    },
  ];

  let profileItems = [
    {
      label: t("MyProfile"),
      icon: "pi pi-user",
      command: () => {
        navigate("/app/profile");
      },
    },
    {
      label: t("ChangeEmailAddress"),
      icon: "pi pi-inbox",
      command: () => {
        navigate("/app/change-email");
      },
    },
    {
      label: t("ChangePassword"),
      icon: "pi pi-cog",
      command: () => {
        navigate("/app/change-password");
      },
    },
    {
      label: t("Logout"),
      icon: "pi pi-sign-out",
      command: () => {
        auth.removeUserFromStorage();
        navigate("/login", { replace: true });
      },
    },
  ];

  return (
    <div className="layout-topbar">
      <a href="/" className="layout-topbar-logo">
        <img
          src={`/images/logo-${
            layoutConfig.colorScheme !== "light" ? "white" : "dark"
          }.svg`}
          width="47.22px"
          height={"35px"}
          alt="logo"
        />
        <span>React.js Boilerplate</span>
      </a>

      <button
        ref={menubuttonRef}
        type="button"
        className="p-link layout-menu-button layout-topbar-button"
        onClick={onMenuToggle}
      >
        <i className="pi pi-bars" />
      </button>

      <button
        ref={topbarmenubuttonRef}
        type="button"
        className="p-link layout-topbar-menu-button layout-topbar-button"
        onClick={showProfileSidebar}
      >
        <i className="pi pi-ellipsis-v" />
      </button>

      <div
        ref={topbarmenuRef}
        className={classNames("layout-topbar-menu", {
          "layout-topbar-menu-mobile-active": layoutState.profileSidebarVisible,
        })}
      >
        <div className="flex justify-content-center align-items-center">
          <Menu model={languages} popup ref={languagesMenu} />
          <div
            className="flex flex-row gap-2 align-items-center cursor-pointer"
            onClick={(e) => languagesMenu?.current?.toggle(e)}
          >
            <span className={`flag flag-${selectedLang}`} />
            <i className="pi pi-chevron-down"></i>
          </div>
        </div>

        <div className="flex justify-content-center align-items-center">
          <Menu model={profileItems} popup ref={profileMenu} />
          <div
            className="flex flex-row gap-2 align-items-center cursor-pointer"
            onClick={(e) => profileMenu?.current?.toggle(e)}
          >
            <button type="button" className="p-link layout-topbar-button">
              <i className="pi pi-user"></i>
              <span>{t("Profile")}</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
});

AppTopbar.displayName = "AppTopbar";

export default AppTopbar;
