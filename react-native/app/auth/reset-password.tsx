import { hideLoading, showLoading } from "@/redux/ui/ui-slice";
import {
  ResetPasswordDto,
  resetPasswordSchema,
} from "@/services/account/account.schemas";
import AccountService from "@/services/account/account.service";
import { showSuccessToast } from "@/utils/toast";
import { Feather, Ionicons } from "@expo/vector-icons";
import { zodResolver } from "@hookform/resolvers/zod";
import { useLocalSearchParams, useRouter } from "expo-router";
import React, { useEffect, useRef, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import {
  Keyboard,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import { useDispatch } from "react-redux";

const CODE_LENGTH = 6;
const RESEND_TIMEOUT = 300; // 5 minutes

const ResetPasswordScreen = () => {
  const router = useRouter();
  const dispatch = useDispatch();
  const { t } = useTranslation();
  const inputRefs = useRef<Array<TextInput | null>>([]);
  const [secondsLeft, setSecondsLeft] = useState(RESEND_TIMEOUT);
  const { email } = useLocalSearchParams<{ email: string }>();

  const {
    control,
    handleSubmit,
    setValue,
    watch,
    formState: { errors },
    reset,
  } = useForm<ResetPasswordDto>({
    resolver: zodResolver(resetPasswordSchema),
    defaultValues: {
      email: email,
      token: "",
      newPassword: "",
      confirmPassword: "",
    },
  });

  const token = watch("token");

  useEffect(() => {
    if (secondsLeft <= 0) return;
    const timer = setInterval(() => {
      setSecondsLeft((prev) => prev - 1);
    }, 1000);
    return () => clearInterval(timer);
  }, [secondsLeft]);

  const formatTime = (sec: number) => {
    const m = Math.floor(sec / 60)
      .toString()
      .padStart(2, "0");
    const s = (sec % 60).toString().padStart(2, "0");
    return `${m}:${s}`;
  };

  const handleCodeChange = (text: string, index: number) => {
    if (!text.match(/^[0-9]$/)) return;

    const codeArray = token.split("");
    codeArray[index] = text;
    const newCode = codeArray.join("").padEnd(CODE_LENGTH, "");
    setValue("token", newCode);

    if (index < CODE_LENGTH - 1) {
      inputRefs.current[index + 1]?.focus();
    } else {
      Keyboard.dismiss();
    }
  };

  const handleKeyPress = (e: any, index: number) => {
    if (e.nativeEvent.key === "Backspace") {
      const codeArray = token.split("");
      codeArray[index] = "";
      if (index > 0) {
        inputRefs.current[index - 1]?.focus();
      } else {
        inputRefs.current[0]?.focus();
      }

      const newCode = codeArray.join("").padEnd(CODE_LENGTH, "");
      setValue("token", newCode);
    }
  };

  const onSubmit = async (data: ResetPasswordDto) => {
    const res = await AccountService.ResetPassword(
      data.email,
      data.token,
      data.newPassword
    );

    if (!res.error) {
      showSuccessToast(t("password-reset-success"));
      router.push("/auth/login");
    }
  };

  const handleResend = async () => {
    if (secondsLeft > 0) return;

    dispatch(showLoading());

    const res = await AccountService.ResetPasswordRequest(email);

    if (!res.error) {
      showSuccessToast(t("code-send-success"));
      setValue("token", "");
      setSecondsLeft(RESEND_TIMEOUT);
      inputRefs.current[0]?.focus();
    }
    dispatch(hideLoading());
  };

  return (
    <View className="flex-1 justify-center items-center bg-blue-100 px-4">
      <TouchableOpacity
        onPress={() => {
          reset();
          router.back();
        }}
        style={{ position: "absolute", top: 40, left: 20 }}
      >
        <Ionicons name="arrow-back" size={24} color="black" />
      </TouchableOpacity>

      <View className="w-full max-w-md p-6 bg-white rounded-2xl shadow-md items-center">
        <Text className="text-2xl font-bold mb-2 text-center">
          {t("reset-password")}
        </Text>

        <Text className="text-sm text-gray-600 mb-4">
          {t("time-left")}{" "}
          <Text className="font-semibold text-black">
            {formatTime(secondsLeft)}
          </Text>
        </Text>

        <Text className="text-center text-gray-600 mb-6">
          {t("enter-verification-code-msg")}
        </Text>

        <Controller
          control={control}
          name="token"
          render={() => (
            <View className="flex-row justify-between w-full">
              {[...Array(CODE_LENGTH)].map((_, index) => (
                <TextInput
                  key={index}
                  ref={(ref) => {
                    inputRefs.current[index] = ref;
                  }}
                  value={token[index] || ""}
                  onChangeText={(text) => handleCodeChange(text, index)}
                  onKeyPress={(e) => handleKeyPress(e, index)}
                  keyboardType="number-pad"
                  maxLength={1}
                  className={`w-12 h-14 border rounded-xl text-center text-xl font-bold text-black bg-gray-100 mx-1 ${errors.token ? "border-red-500" : "border-gray-400"}`}
                  selectionColor="#000"
                />
              ))}
            </View>
          )}
        />

        <Controller
          control={control}
          name="newPassword"
          render={({ field: { onChange, onBlur, value } }) => (
            <View
              className={`flex-row items-center border p-3 mt-5 border-gray-400 bg-gray-100 rounded-xl text-black ${errors.newPassword ? "border-red-500" : "border-gray-400"}`}
            >
              <Feather name="lock" size={20} color="#000" className="mr-2" />
              <TextInput
                className="flex-1 text-black h-8"
                placeholder={t("new-password")}
                placeholderTextColor={"#000"}
                onBlur={onBlur}
                onChangeText={onChange}
                value={value}
                secureTextEntry={true}
                keyboardType={"default"}
              />
            </View>
          )}
        />

        <Controller
          control={control}
          name="confirmPassword"
          render={({ field: { onChange, onBlur, value } }) => (
            <View
              className={`flex-row items-center border border-gray-400 p-3 bg-gray-100 rounded-xl mt-5 ${errors.confirmPassword ? "border-red-500" : "border-gray-400"}`}
            >
              <Feather name="lock" size={20} color="#000" className="mr-2" />
              <TextInput
                className="flex-1 text-black h-8"
                placeholder={t("confirm-password")}
                placeholderTextColor="#000"
                onBlur={onBlur}
                onChangeText={onChange}
                value={value}
                secureTextEntry={true}
                keyboardType={"default"}
              />
            </View>
          )}
        />

        <TouchableOpacity
          onPress={handleSubmit(onSubmit)}
          className="p-4 w-full rounded-full mt-10 shadow active:opacity-90 bg-blue-400"
        >
          <Text className="text-white font-bold text-center text-lg">
            {t("reset-my-password")}
          </Text>
        </TouchableOpacity>

        <TouchableOpacity
          onPress={handleResend}
          disabled={secondsLeft > 0}
          className={`p-3 rounded-full w-full border mt-3 ${
            secondsLeft > 0
              ? "border-gray-300 bg-gray-100"
              : "border-blue-500 bg-white"
          }`}
        >
          <Text
            className={`text-center font-medium ${
              secondsLeft > 0 ? "text-gray-400" : "text-blue-500"
            }`}
          >
            {t("resend-code")}
          </Text>
        </TouchableOpacity>
      </View>
    </View>
  );
};

export default ResetPasswordScreen;
