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

const RegisterScreen = () => {
  const router = useRouter();
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
    if (response.error) {
      Alert.alert(
        "Hata",
        response.error.errors.join(", ") || "Kayıt işlemi başarısız oldu."
      );
      return;
    }

    Alert.alert("Başarılı", "Kayıt başarılı!");
    reset();
    router.push("/auth/login");
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
          <View className="w-full max-w-xl bg-blue-50 p-6 rounded-2xl shadow-md">
            <Text className="text-2xl font-bold text-center mb-6">
              Kayıt Ol
            </Text>

            {[
              {
                name: "firstName",
                placeholder: "Ad",
                keyboard: "default" as const,
                icon: "user" as const,
              },
              {
                name: "lastName",
                placeholder: "Soyad",
                keyboard: "default" as const,
                icon: "user" as const,
              },
              {
                name: "phoneNumber",
                placeholder: "Telefon Numarası",
                keyboard: "phone-pad" as const,
                icon: "phone" as const,
              },
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
                  name={name as keyof CreateUserDto}
                  render={({ field: { onChange, onBlur, value } }) => (
                    <View
                      className={`flex-row items-center border border-gray-300 p-3 rounded-full bg-white mt-3 ${errors[name as keyof CreateUserDto] ? "border-red-500" : ""}`}
                    >
                      <Feather
                        name={icon}
                        size={20}
                        color="#888"
                        className="mr-2"
                      />
                      <TextInput
                        className="flex-1 text-base text-black"
                        placeholder={placeholder}
                        onBlur={onBlur}
                        onChangeText={onChange}
                        value={value}
                        secureTextEntry={secure}
                        keyboardType={keyboard}
                        placeholderTextColor="#aaa"
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
              className="bg-blue-500 p-4 rounded-full mt-4 shadow active:opacity-90"
              onPress={handleSubmit(onSubmit)}
              disabled={isSubmitting}
            >
              <Text className="text-white font-bold text-lg text-center">
                {isSubmitting ? "Gönderiliyor..." : "Kayıt Ol"}
              </Text>
            </TouchableOpacity>

            <Text
              className="text-lg text-center mt-3 text-blue-600"
              onPress={() => router.push("/auth/login")}
            >
              Zaten bir hesabınız var mı? Giriş Yapın.
            </Text>
          </View>
        </View>
      </ScrollView>
    </KeyboardAvoidingView>
  );
};

export default RegisterScreen;
