import { fetchUserInfo, login } from "@/redux/auth/auth-slice";
import { AppDispatch } from "@/redux/store";
import { hideLoading, showLoading } from "@/redux/ui/ui-slice";
import { LoginDto, loginSchema } from "@/services/auth/auth.schemas";
import { Feather } from "@expo/vector-icons";
import { zodResolver } from "@hookform/resolvers/zod";
import { useRouter } from "expo-router";
import React from "react";
import { Controller, useForm } from "react-hook-form";
import {
  Alert,
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
    } else {
      Alert.alert(
        "Giriş Hatası",
        (resultAction.payload as string) || "Bir hata oluştu"
      );
    }

    dispatch(hideLoading());
  };

  return (
    <KeyboardAvoidingView
      className="flex-1 bg-blue-100"
      behavior={Platform.OS === "ios" ? "padding" : undefined}
    >
      <ScrollView
        contentContainerStyle={{ flexGrow: 1 }}
        keyboardShouldPersistTaps="handled"
      >
        <View className="flex-1 justify-center items-center px-4">
          <View className="w-full max-w-xl p-6 rounded-2xl shadow-md bg-blue-50">
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
                      className={`flex-row items-center border border-blue-700 p-3 rounded-full mt-3 ${errors[name as keyof LoginDto] ? "border-red-500" : "border-blue-700"}`}
                    >
                      <Feather
                        name={icon}
                        size={20}
                        color="#1d4ed8"
                        className="mr-2"
                      />
                      <TextInput
                        className="flex-1 text-base text-blue-700"
                        placeholder={placeholder}
                        placeholderClassName="text-blue-700"
                        placeholderTextColor="#1d4ed8"
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
              className="p-4 rounded-full mt-4 shadow active:opacity-90 bg-blue-400"
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
          </View>
        </View>
      </ScrollView>
    </KeyboardAvoidingView>
  );
};

export default RegisterScreen;
