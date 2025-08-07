import { AppDispatch } from "@/redux/store";
import { hideLoading, showLoading } from "@/redux/ui/ui-slice";
import AccountService from "@/services/account/account.service";
import { showSuccessToast } from "@/utils/toast";
import { Feather, Ionicons } from "@expo/vector-icons";
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
import z from "zod";

const ForgotPasswordScreen = () => {
  const router = useRouter();
  const dispatch = useDispatch<AppDispatch>();

  const schema = z.object({
    email: z.email("Geçerli bir e-posta girin"),
  });

  const {
    control,
    handleSubmit,
    setValue,
    watch,
    formState: { errors },
    reset,
  } = useForm<{
    email: string;
  }>({
    resolver: zodResolver(schema),
    defaultValues: {
      email: "",
    },
  });

  const onSubmit = async (data: { email: string }) => {
    dispatch(showLoading());

    const res = await AccountService.ResetPasswordRequest(data.email);

    if (!res.error) {
      showSuccessToast("Password reset link sent to your email.");

      router.push({
        pathname: "/auth/reset-password",
        params: { email: data.email },
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
          <TouchableOpacity
            onPress={() => {
              reset();
              router.back();
            }}
            style={{ position: "absolute", top: 40, left: 20 }}
          >
            <Ionicons name="arrow-back" size={24} color="black" />
          </TouchableOpacity>

          <View className="w-full max-w-xl p-6 rounded-2xl shadow-md bg-white">
            <Text className="text-2xl font-bold text-center">
              Parola Sıfırlama
            </Text>

            <Controller
              control={control}
              name="email"
              render={({ field: { onChange, onBlur, value } }) => (
                <View
                  className={`flex-row items-center p-3 mt-7 border bg-gray-100 rounded-xl text-black ${errors.email ? "border-red-500" : "border-gray-400"}`}
                >
                  <Feather
                    name={"mail"}
                    size={20}
                    color="#000"
                    className="mr-2"
                  />
                  <TextInput
                    className="flex-1 text-black h-8"
                    placeholder={"E-posta Adresi"}
                    placeholderTextColor="#000"
                    onChangeText={onChange}
                    onBlur={onBlur}
                    value={value}
                    keyboardType={"email-address"}
                  />
                </View>
              )}
            />

            <TouchableOpacity
              className="p-4 rounded-full mt-10 shadow active:opacity-90 bg-blue-400"
              onPress={handleSubmit(onSubmit)}
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
