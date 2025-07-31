import { configureStore } from "@reduxjs/toolkit";
import authSliceReducer from "./auth/auth-slice";
import themeSliceReducer from "./theme/theme-slice";
import uiSliceReducer from "./ui/ui-slice";

export const store = configureStore({
  reducer: {
    auth: authSliceReducer,
    theme: themeSliceReducer,
    ui: uiSliceReducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: false,
    }),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
