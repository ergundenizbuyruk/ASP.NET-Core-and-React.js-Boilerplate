import { RootState } from "@/redux/store";
import React from "react";
import { ActivityIndicator, StyleSheet, View } from "react-native";
import { useSelector } from "react-redux";

const GlobalSpinner = () => {
  const loading = useSelector((state: RootState) => state.ui.globalLoading);

  if (!loading) return null;

  return (
    <View style={styles.overlay}>
      <ActivityIndicator size="large" color="#fff" />
    </View>
  );
};

const styles = StyleSheet.create({
  overlay: {
    position: "absolute",
    top: 0,
    left: 0,
    height: "100%",
    width: "100%",
    zIndex: 9999,
    backgroundColor: "rgba(0,0,0,0.3)",
    justifyContent: "center",
    alignItems: "center",
  },
});

export default GlobalSpinner;
