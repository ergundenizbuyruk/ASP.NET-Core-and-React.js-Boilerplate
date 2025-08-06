import { hideLoading, showLoading } from "@/redux/ui/ui-slice";
import AccountService from "@/services/account/account.service";
import { showErrorToast, showSuccessToast } from "@/utils/toast";
import { useLocalSearchParams, useRouter } from "expo-router";
import React, { useEffect, useRef, useState } from "react";
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

const VerifyCodeScreen = () => {
  const router = useRouter();
  const dispatch = useDispatch();
  const [code, setCode] = useState<string[]>(Array(CODE_LENGTH).fill(""));
  const inputRefs = useRef<Array<TextInput | null>>([]);
  const [secondsLeft, setSecondsLeft] = useState(RESEND_TIMEOUT);
  const { email } = useLocalSearchParams<{ email: string }>();

  if (!email) {
    showErrorToast("Email address is required to verify the code.");
    return null;
  }

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

  const handleChange = (text: string, index: number) => {
    if (!text.match(/^[0-9]$/)) return;

    const newCode = [...code];
    newCode[index] = text;
    setCode(newCode);

    if (index < CODE_LENGTH - 1) {
      inputRefs.current[index + 1]?.focus();
    } else {
      Keyboard.dismiss();
    }
  };

  const handleKeyPress = (e: any, index: number) => {
    if (e.nativeEvent.key === "Backspace") {
      const newCode = [...code];

      const prevValue = newCode[index];
      if (prevValue) {
        newCode[index] = "";
      } else {
        if (index > 0) {
          inputRefs.current[index - 1]?.focus();
        } else {
          inputRefs.current[0]?.focus();
        }
      }

      setCode(newCode);
    }
  };

  const handleSubmit = async () => {
    const finalCode = code.join("");
    if (finalCode.length !== CODE_LENGTH) {
      showErrorToast("Please enter the 6-digit code.", "Invalid Code");
      return;
    }

    const res = await AccountService.ConfirmEmail(email, finalCode);

    if (!res.error) {
      showSuccessToast("Code verified successfully.");
      router.push("/auth/login");
    }
  };

  const handleResend = async () => {
    if (secondsLeft > 0) return;

    dispatch(showLoading());

    const res = await AccountService.EmailConfirmationTokenRequest(email);

    if (!res.error) {
      showSuccessToast("Code sent successfully.");
      setCode(Array(CODE_LENGTH).fill(""));
      setSecondsLeft(RESEND_TIMEOUT);
      inputRefs.current[0]?.focus();
    }
    dispatch(hideLoading());
  };

  return (
    <View className="flex-1 justify-center items-center bg-blue-100 px-4">
      <View className="w-full max-w-md p-6 bg-white rounded-2xl shadow-md items-center">
        <Text className="text-2xl font-bold mb-2 text-center">
          Enter Verification Code
        </Text>

        <Text className="text-sm text-gray-600 mb-4">
          Time Left:{" "}
          <Text className="font-semibold text-black">
            {formatTime(secondsLeft)}
          </Text>
        </Text>

        <Text className="text-center text-gray-600 mb-6">
          Enter the 6-digit verification code sent to your email address.
        </Text>

        <View className="flex-row justify-between w-full mb-6">
          {code.map((digit, index) => (
            <TextInput
              key={index}
              ref={(ref) => {
                inputRefs.current[index] = ref;
              }}
              value={digit}
              onChangeText={(text) => handleChange(text, index)}
              onKeyPress={(e) => handleKeyPress(e, index)}
              keyboardType="number-pad"
              maxLength={1}
              className="w-12 h-14 border border-gray-400 rounded-xl text-center text-xl font-bold text-black bg-gray-100 mx-1"
              selectionColor="#000"
            />
          ))}
        </View>

        <TouchableOpacity
          onPress={handleSubmit}
          className="p-4 rounded-full mt-10 shadow active:opacity-90 bg-blue-400"
        >
          <Text className="text-white font-bold text-center text-lg">
            Verify
          </Text>
        </TouchableOpacity>

        <TouchableOpacity
          onPress={handleResend}
          disabled={secondsLeft > 0}
          className={`p-3 rounded-full w-full border ${
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
            Resend Code
          </Text>
        </TouchableOpacity>
      </View>
    </View>
  );
};

export default VerifyCodeScreen;
