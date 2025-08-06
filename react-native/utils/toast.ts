import Toast from "react-native-toast-message";

export const showSuccessToast = (
  message: string,
  title: string = "Success"
) => {
  Toast.show({
    type: "success",
    text1: title,
    text2: message,
    position: "top",
  });
};

export const showErrorToast = (message: string, title: string = "Error") => {
  Toast.show({
    type: "error",
    text1: title,
    text2: message,
    position: "top",
  });
};

export const showInfoToast = (message: string, title: string = "İnfo") => {
  Toast.show({
    type: "info",
    text1: title,
    text2: message,
    position: "top",
  });
};
