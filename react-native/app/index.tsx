import { fetchUserInfo, loadTokensFromStorage } from "@/redux/auth/auth-slice";
import { AppDispatch } from "@/redux/store";
import { hideLoading, showLoading } from "@/redux/ui/ui-slice";
import { useRouter } from "expo-router";
import { useEffect } from "react";
import { ActivityIndicator, View } from "react-native";
import { useDispatch } from "react-redux";

export default function SplashScreen() {
  const router = useRouter();
  const dispatch = useDispatch<AppDispatch>();

  useEffect(() => {
    async function initializeApp() {
      dispatch(showLoading());
      const authResult = await dispatch(loadTokensFromStorage());

      if (loadTokensFromStorage.fulfilled.match(authResult)) {
        await dispatch(fetchUserInfo());
        router.replace("/dashboard");
      } else {
        router.replace("/auth/login");
      }
      dispatch(hideLoading());
    }

    initializeApp();
  }, []);

  return (
    <View className={`flex-1 items-center justify-center`}>
      <ActivityIndicator size="large" />
    </View>
  );
}
