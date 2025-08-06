import { fetchUserInfo, login } from "@/redux/auth/auth-slice";
import { AppDispatch } from "@/redux/store";
import { hideLoading, showLoading } from "@/redux/ui/ui-slice";
import AccountService from "@/services/account/account.service";
import { LoginDto, loginSchema } from "@/services/auth/auth.schemas";
import { showInfoToast } from "@/utils/toast";
import { Feather } from "@expo/vector-icons";
import { zodResolver } from "@hookform/resolvers/zod";
import { useRouter } from "expo-router";
import React from "react";
import { Controller, useForm } from "react-hook-form";
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

const RegisterScreen = () => {
  const router = useRouter();
  const dispatch = useDispatch<AppDispatch>();

  const {
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<LoginDto>({
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = async (data: LoginDto) => {
    dispatch(showLoading());

    const resultAction = await dispatch(
      login({ email: data.email, password: data.password })
    );

    if (login.fulfilled.match(resultAction)) {
      await dispatch(fetchUserInfo());
      router.push("/dashboard");
      reset();
    }

    if (
      typeof resultAction.payload === "string" &&
      resultAction.payload?.includes("e-posta adresinizi onaylayınız")
    ) {
      const res = await AccountService.EmailConfirmationTokenRequest(
        data.email
      );
      if (!res.error) {
        showInfoToast(
          "Lütfen e-posta adresinizi onaylayınız.",
          "E-posta Onayı"
        );
        router.push({
          pathname: "/auth/verify-code",
          params: { email: data.email },
        });
        reset();
      }
    }

    dispatch(hideLoading());
  };

  return (
    <KeyboardAvoidingView
      className="flex-1"
      behavior={Platform.OS === "ios" ? "padding" : undefined}
    >
      <ScrollView
        contentContainerStyle={{ flexGrow: 1 }}
        keyboardShouldPersistTaps="handled"
      >
        <View className="flex-1 justify-center items-center px-4 bg-blue-100">
          <View className="w-full max-w-xl p-6 rounded-2xl shadow-md bg-white">
            <Text className="text-2xl font-bold text-center mb-6">
              Giriş Yap
            </Text>
            {[
              {
                name: "email",
                placeholder: "E-posta",
                keyboard: "email-address" as const,
                icon: "mail" as const,
              },
              {
                name: "password",
                placeholder: "Parola",
                secure: true,
                keyboard: "default" as const,
                icon: "lock" as const,
              },
            ].map(({ name, placeholder, secure, keyboard, icon }) => (
              <View key={name} className="mb-4">
                <Controller
                  control={control}
                  name={name as keyof LoginDto}
                  render={({ field: { onChange, onBlur, value } }) => (
                    <View
                      className={`flex-row items-center mt-3 p-3 border border-gray-400 bg-gray-100 rounded-xl text-black ${errors[name as keyof LoginDto] ? "border-red-500" : "border-gray-400"}`}
                    >
                      <Feather
                        name={icon}
                        size={20}
                        color="#000"
                        className="mr-2"
                      />
                      <TextInput
                        className="flex-1 text-black h-8"
                        placeholder={placeholder}
                        placeholderTextColor="#000"
                        onBlur={onBlur}
                        onChangeText={onChange}
                        value={value}
                        secureTextEntry={secure}
                        keyboardType={keyboard}
                      />
                    </View>
                  )}
                />
                {/* {errors[name as keyof CreateUserDto] && (
                  <Text className="text-red-500 text-sm mt-1">
                    {errors[name as keyof CreateUserDto]?.message?.toString()}
                  </Text>
                )} */}
              </View>
            ))}

            <TouchableOpacity
              className="p-4 mt-5 rounded-full shadow active:opacity-90 bg-blue-400"
              onPress={handleSubmit(onSubmit)}
              disabled={isSubmitting}
            >
              <Text className="text-white font-bold text-lg text-center">
                {isSubmitting ? "Gönderiliyor..." : "Giriş Yap"}
              </Text>
            </TouchableOpacity>

            <Text
              className="text-lg text-center mt-3 font-semibold text-blue-700"
              onPress={() => router.push("/auth/register")}
            >
              Hesabınız yok mu? Kayıt Olun.
            </Text>
            <Text
              className="text-lg text-center mt-2 font-semibold text-blue-700"
              onPress={() => router.push("/auth/forgot-password")}
            >
              Parolanızı mı unuttunuz?
            </Text>
          </View>
        </View>
      </ScrollView>
    </KeyboardAvoidingView>
  );
};

export default RegisterScreen;
