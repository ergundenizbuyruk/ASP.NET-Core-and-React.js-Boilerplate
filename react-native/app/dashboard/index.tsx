import { logout } from "@/redux/auth/auth-slice";
import { AppDispatch, RootState } from "@/redux/store";
import { hideLoading, showLoading } from "@/redux/ui/ui-slice";
import { useRouter } from "expo-router";
import React from "react";
import { Button, Text, View } from "react-native";
import { useDispatch, useSelector } from "react-redux";

const Dashboard: React.FC = () => {
  const auth = useSelector((state: RootState) => state.auth);
  const dispatch = useDispatch<AppDispatch>();
  const router = useRouter();

  return (
    <View className="flex-1 justify-center items-center bg-blue-100">
      <Text className="text-2xl font-bold">Dashboard</Text>
      <Text className="text-xl font-semibold mt-4">
        Hello {auth.user?.firstName} {auth.user?.lastName}
      </Text>

      <Button
        title="Logout"
        onPress={async () => {
          dispatch(showLoading());
          await dispatch(logout());
          router.replace("/auth/login");
          dispatch(hideLoading());
        }}
      />
    </View>
  );
};

export default Dashboard;
