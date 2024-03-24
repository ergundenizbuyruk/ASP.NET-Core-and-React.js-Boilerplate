import { useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import accountService from "../../services/account/account.service";
import { ConfirmNewEmailDto } from "../../services/account/account.dto";
import { useToast } from "../../utils/toast";
import { useTranslation } from "react-i18next";
import { useAuth } from "../../utils/auth";

const NewEmailConfirmPage = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const toast = useToast();
  const auth = useAuth();
  const { t } = useTranslation();

  const token = searchParams.get("token") || "";
  const oldEmail = searchParams.get("oldEmail") || "";
  const newEmail = searchParams.get("newEmail") || "";

  if (!token || !oldEmail || !newEmail) {
    navigate("/login");
  }

  useEffect(() => {
    if (
      token === undefined ||
      token === "" ||
      oldEmail === undefined ||
      oldEmail === "" ||
      newEmail === undefined ||
      newEmail === ""
    ) {
      navigate("/login");
      return;
    }

    const confirmNewEmailDto: ConfirmNewEmailDto = {
      token,
      oldEmail,
      newEmail,
    };

    accountService.ConfirmNewEmail(confirmNewEmailDto).then((res) => {
      if (res.result && !res.result?.error) {
        toast.show(t("EmailConfirmed"), "success");
        auth.removeUserFromStorage();
      }

      navigate("/login", { replace: true });
    });
  }, [token, oldEmail, newEmail]);

  return <></>;
};

export default NewEmailConfirmPage;
