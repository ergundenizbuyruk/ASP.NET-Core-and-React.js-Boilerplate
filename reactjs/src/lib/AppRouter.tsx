import { Route, Routes } from "react-router-dom";
import App from "../App";
import Login from "../pages/login/Login";
import Register from "../pages/register/Register";
import LandingPage from "../pages/landing/LandingPage";
import Homepage from "../pages/homepage/Homepage";
import ResetPassword from "../pages/reset-password/ResetPassword";

const AppRouter = () => {
  return (
    <Routes>
      <Route path="/" element={<LandingPage />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/reset-password" element={<ResetPassword />} />
      <Route path="/app" element={<App />}>
        <Route path="homepage" element={<Homepage />} />
      </Route>
    </Routes>
  );
};

export default AppRouter;
