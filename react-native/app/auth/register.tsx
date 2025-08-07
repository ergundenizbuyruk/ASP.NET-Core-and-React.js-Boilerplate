import {
  CreateUserDto,
  createUserSchema,
} from "@/services/account/account.schemas";
import AccountService from "@/services/account/account.service";
import { Feather } from "@expo/vector-icons";
import { zodResolver } from "@hookform/resolvers/zod";
import { useRouter } from "expo-router";
import React from "react";
import { Controller, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import {
  KeyboardAvoidingView,
  Platform,
  ScrollView,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";

const RegisterScreen = () => {
  const router = useRouter();
  const { t } = useTranslation();
  const {
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<CreateUserDto>({
    resolver: zodResolver(createUserSchema),
  });

  const onSubmit = async (data: CreateUserDto) => {
    const response = await AccountService.Register(data);
    if (response.data) {
      router.push({
        pathname: "/auth/verify-code",
        params: { email: response.data.email },
      });
      reset();
    }
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
          <View className="w-full max-w-xl bg-white p-6 rounded-2xl shadow-md">
            <Text className="text-2xl font-bold text-center mb-6">
              {t("register")}
            </Text>

            {[
              {
                name: "firstName",
                placeholder: t("firstName"),
                keyboard: "default" as const,
                icon: "user" as const,
              },
              {
                name: "lastName",
                placeholder: t("lastName"),
                keyboard: "default" as const,
                icon: "user" as const,
              },
              {
                name: "email",
                placeholder: t("email"),
                keyboard: "email-address" as const,
                icon: "mail" as const,
              },
              {
                name: "phoneNumber",
                placeholder: t("phoneNumber"),
                keyboard: "phone-pad" as const,
                icon: "phone" as const,
              },
              {
                name: "password",
                placeholder: t("password"),
                secure: true,
                keyboard: "default" as const,
                icon: "lock" as const,
              },
            ].map(({ name, placeholder, secure, keyboard, icon }) => (
              <View key={name} className="mb-4">
                <Controller
                  control={control}
                  name={name as keyof CreateUserDto}
                  render={({ field: { onChange, onBlur, value } }) => (
                    <View
                      className={`flex-row items-center mt-3 p-3 border border-gray-400 bg-gray-100 rounded-xl text-black ${errors[name as keyof CreateUserDto] ? "border-red-500" : ""}`}
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
                        onBlur={onBlur}
                        onChangeText={onChange}
                        value={value}
                        secureTextEntry={secure}
                        keyboardType={keyboard}
                        placeholderTextColor="#000"
                      />
                    </View>
                  )}
                />
              </View>
            ))}

            <TouchableOpacity
              className="p-4 rounded-full mt-5 shadow active:opacity-90 bg-blue-400"
              onPress={handleSubmit(onSubmit)}
              disabled={isSubmitting}
            >
              <Text className="text-white font-bold text-lg text-center">
                {isSubmitting ? t("sending") : t("register")}
              </Text>
            </TouchableOpacity>

            <Text
              className="text-lg text-center mt-3 text-blue-600"
              onPress={() => router.push("/auth/login")}
            >
              {t("login-msg")}
            </Text>
          </View>
        </View>
      </ScrollView>
    </KeyboardAvoidingView>
  );
};

export default RegisterScreen;
