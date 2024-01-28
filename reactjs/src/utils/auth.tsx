import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import axios from "axios";
import { jwtDecode } from "jwt-decode";
import { AccessTokenDto } from "../services/auth/auth.dto";

interface User {
  id: string;
  email: string;
  username: string;
  permissions: string[];
  exp: Date;
  accessToken: string;
  refreshToken: string;
}

interface AuthContextModel {
  getUser: () => User;
  setToken: (input: AccessTokenDto, remember: boolean) => void;
  removeUserFromStorage: () => void;
}

let AuthContext = React.createContext<AuthContextModel>(null!);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  var user: User = {
    id: "",
    email: "",
    username: "",
    permissions: [],
    exp: new Date(),
    accessToken: "",
    refreshToken: "",
  };

  const setUserFromStorage = () => {
    user.accessToken = localStorage.getItem("accessToken") || "";
    user.refreshToken = localStorage.getItem("refreshToken") || "";

    if (user.accessToken.length == 0) {
      user.accessToken = sessionStorage.getItem("accessToken") || "";
    }

    if (user.refreshToken.length == 0) {
      user.refreshToken = sessionStorage.getItem("refreshToken") || "";
    }

    if (user.accessToken.length > 0) {
      const decoded = jwtDecode(user.accessToken) as any;

      user.id =
        decoded[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        ];

      user.email = decoded["email"];
      user.username =
        decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
      user.exp = new Date(decoded["exp"] * 1000);

      let rawPermissions = decoded["Permission"];
      if (rawPermissions) {
        user.permissions = rawPermissions;
      }
    } else {
      user = {
        id: "",
        email: "",
        username: "",
        permissions: [],
        exp: new Date(),
        accessToken: "",
        refreshToken: "",
      };
    }

    axios.defaults.headers.common["Authorization"] =
      "Bearer " + user.accessToken;
  };

  const removeUserFromStorage = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    sessionStorage.removeItem("accessToken");
    sessionStorage.removeItem("refreshToken");
    setUserFromStorage();
  };

  const setToken = (input: AccessTokenDto, remember: boolean) => {
    if (remember) {
      if (input.accessToken) {
        localStorage.setItem("accessToken", input.accessToken);
      }
      if (input.refreshToken) {
        localStorage.setItem("refreshToken", input.refreshToken);
      }
    } else {
      if (input.accessToken) {
        sessionStorage.setItem("accessToken", input.accessToken);
      }
      if (input.refreshToken) {
        sessionStorage.setItem("refreshToken", input.refreshToken);
      }
    }

    setUserFromStorage();
  };

  const getUser = () => {
    setUserFromStorage();
    return user;
  };

  const value = { getUser, setToken, removeUserFromStorage };

  setUserFromStorage();

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  return React.useContext(AuthContext);
}

export function HasPermission({
  permissions = [],
  children,
}: {
  permissions?: string[];
  children: JSX.Element;
}) {
  let auth = useAuth();
  let location = useLocation();
  var user = auth.getUser();

  if (user.accessToken.length == 0) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (permissions && permissions.length > 0) {
    for (let permission in permissions) {
      if (!user.permissions.includes(permission)) {
        return <Navigate to={"/access-denied"} replace={false} />;
      }
    }
  } else {
    return children;
  }

  return children;
}
