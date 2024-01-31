import { Route, Routes } from "react-router-dom";
import App from "../App";
import Login from "../pages/login/Login";
import Register from "../pages/register/Register";
import LandingPage from "../pages/landing/LandingPage";
import Homepage from "../pages/homepage/Homepage";
import ResetPassword from "../pages/reset-password/ResetPassword";
import { HasPermission } from "../utils/auth";
import { Permission } from "../services/auth/permissions";
import NotFoundPage from "../pages/not-found/NotFound";
import AccessDeniedPage from "../pages/access-denied/AccessDeniedPage";
import EmailConfirmPage from "../pages/auth/EmailConfirm";
import ChangeEmailPage from "../pages/auth/ChangeEmail";
import NewEmailConfirmPage from "../pages/auth/NewEmailConfirm";
import ChangePasswordPage from "../pages/auth/ChangePassword";
import ProfilePage from "../pages/auth/Profile";
import RolePage from "../pages/role/Role";

const AppRouter = () => {
  return (
    <Routes>
      <Route path="/" element={<LandingPage />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/reset-password" element={<ResetPassword />} />
      <Route path="/email-confirm" element={<EmailConfirmPage />} />
      <Route path="/change-email-confirm" element={<NewEmailConfirmPage />} />
      <Route path="/app" element={<App />}>
        <Route
          path="profile"
          element={
            <HasPermission>
              <ProfilePage />
            </HasPermission>
          }
        />
        <Route
          path="homepage"
          element={
            <HasPermission>
              <Homepage />
            </HasPermission>
          }
        />
        <Route
          path="change-email"
          element={
            <HasPermission>
              <ChangeEmailPage />
            </HasPermission>
          }
        />
        <Route
          path="change-password"
          element={
            <HasPermission>
              <ChangePasswordPage />
            </HasPermission>
          }
        />
        <Route
          path="roles"
          element={
            <HasPermission permissions={[Permission.RoleDefault]}>
              <RolePage />
            </HasPermission>
          }
        />
      </Route>
      <Route path="/access-denied" element={<AccessDeniedPage />} />
      <Route path="/not-found" element={<NotFoundPage />} />
    </Routes>
  );
};

export default AppRouter;
