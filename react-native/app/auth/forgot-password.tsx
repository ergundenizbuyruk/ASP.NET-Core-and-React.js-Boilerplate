import { AppDispatch } from "@/redux/store";
import { hideLoading, showLoading } from "@/redux/ui/ui-slice";
import AccountService from "@/services/account/account.service";
import { showErrorToast, showSuccessToast } from "@/utils/toast";
import { Feather } from "@expo/vector-icons";
import { useRouter } from "expo-router";
import React from "react";
import {
  KeyboardAvoidingView,
  Platform,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import { useDispatch } from "react-redux";

const ForgotPasswordScreen = () => {
  const router = useRouter();
  const dispatch = useDispatch<AppDispatch>();
  const [email, setEmail] = React.useState("");

  const handleSubmit = async () => {
    if (!email) {
      showErrorToast("Please enter your email address.");
      return;
    }

    dispatch(showLoading());

    const res = await AccountService.ResetPasswordRequest(email);

    if (!res.error) {
      showSuccessToast("Password reset link sent to your email.");

      router.push({
        pathname: "/auth/reset-password",
        params: { email: email },
      });
    }

    dispatch(hideLoading());
  };

  return (
    <KeyboardAvoidingView
      className="flex-1 "
      behavior={Platform.OS === "ios" ? "padding" : undefined}
    >
      <ScrollView
        contentContainerStyle={{ flexGrow: 1 }}
        keyboardShouldPersistTaps="handled"
      >
        <View className="flex-1 justify-center items-center px-4 bg-blue-100">
          <View className="w-full max-w-xl p-6 rounded-2xl shadow-md bg-white">
            <Text className="text-2xl font-bold text-center">
              Parola Sıfırlama
            </Text>

            <View
              className={`flex-row items-center p-3 mt-7 border border-gray-400 bg-gray-100 rounded-xl text-black`}
            >
              <Feather name={"mail"} size={20} color="#000" className="mr-2" />
              <TextInput
                className="flex-1 text-black h-8"
                placeholder={"E-posta Adresi"}
                placeholderTextColor="#000"
                onChangeText={(text) => setEmail(text)}
                value={email}
                keyboardType={"email-address"}
              />
            </View>

            <TouchableOpacity
              className="p-4 rounded-full mt-10 shadow active:opacity-90 bg-blue-400"
              onPress={handleSubmit}
              disabled={email === ""}
            >
              <Text className="text-white font-bold text-lg text-center">
                Parolamı Sıfırla
              </Text>
            </TouchableOpacity>
          </View>
        </View>
      </ScrollView>
    </KeyboardAvoidingView>
  );
};

export default ForgotPasswordScreen;
