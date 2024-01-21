import { useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import accountService from "../../services/account/account.service";
import { ConfirmEmailDto } from "../../services/account/account.dto";
import { useToast } from "../../utils/toast";
import { useTranslation } from "react-i18next";

const EmailConfirmPage = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const toast = useToast();
  const { t } = useTranslation();

  const token = searchParams.get("token") || "";
  const userId = searchParams.get("id") || "";

  if (
    token === undefined ||
    token === "" ||
    userId === undefined ||
    userId === ""
  ) {
    navigate("/login");
  }

  useEffect(() => {
    if (
      token === undefined ||
      token === "" ||
      userId === undefined ||
      userId === ""
    ) {
      navigate("/login");
      return;
    }

    const confirmEmailDto: ConfirmEmailDto = {
      token,
      userId,
    };

    accountService.ConfirmEmail(confirmEmailDto).then((res) => {
      if (res) {
        toast.show(t("EmailConfirmed"), "success");
        navigate("/login", { replace: true });
      }
    });
  }, [token, userId]);

  return <></>;
};

export default EmailConfirmPage;
