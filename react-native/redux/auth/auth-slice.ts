import { UserDto } from "@/services/account/account.dto";
import accountService from "@/services/account/account.service";
import authService from "@/services/auth/auth.service";
import { Permission } from "@/services/auth/permissions";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { jwtDecode } from "jwt-decode";

type DecodedToken = {
  id: string;
  email: string;
  roles: string[];
  permissions: number[];
  exp: number;
};

interface AuthState {
  user: UserDto | null;
  accessToken: string | null;
  refreshToken: string | null;
  accessTokenExpiration: Date | null;
  permissions: Permission[] | null;
  roles: string[] | null;
  loading: boolean;
  error: string | null;
  isAuthenticated: boolean;
}

const initialState: AuthState = {
  user: null,
  accessToken: null,
  refreshToken: null,
  accessTokenExpiration: null,
  permissions: null,
  roles: null,
  loading: false,
  error: null,
  isAuthenticated: false,
};

export const login = createAsyncThunk(
  "auth/login",
  async (credentials: { email: string; password: string }, thunkAPI) => {
    try {
      const response = await authService.login(
        credentials.email,
        credentials.password
      );

      console.log("Login response:", response);

      if (response.error) {
        return thunkAPI.rejectWithValue(
          response.error.errors.join(",") || "Login failed"
        );
      }

      const { accessToken, refreshToken } = response.data;
      const decoded: DecodedToken = jwtDecode<DecodedToken>(accessToken);

      const accessTokenExp = new Date(decoded.exp * 1000);

      await AsyncStorage.multiSet([
        ["accessToken", accessToken],
        ["refreshToken", refreshToken],
      ]);

      return {
        accessToken,
        refreshToken,
        accessTokenExpiration: accessTokenExp,
        permissions: decoded.permissions,
        roles: decoded.roles,
        user: {
          id: decoded.id,
          email: decoded.email,
        } as UserDto,
      };
    } catch (err: any) {
      console.log("Login error:", err.response);
      return thunkAPI.rejectWithValue(
        err.response?.data?.error?.errors?.join(",") || "Login failed"
      );
    }
  }
);

export const refreshToken = createAsyncThunk(
  "auth/refreshToken",
  async (_, thunkAPI) => {
    try {
      const refreshToken = await AsyncStorage.getItem("refreshToken");
      if (!refreshToken) throw new Error("No refresh token");

      const response = await authService.refresh({ token: refreshToken });
      if (!response.data) {
        return thunkAPI.rejectWithValue("Failed to refresh token");
      }

      const { accessToken: newAccessToken, refreshToken: newRefreshToken } =
        response.data;
      const decoded: DecodedToken = jwtDecode<DecodedToken>(newAccessToken);

      const accessTokenExp = new Date(decoded.exp * 1000);

      await AsyncStorage.multiSet([
        ["accessToken", newAccessToken],
        ["refreshToken", newRefreshToken],
      ]);

      return {
        accessToken: newAccessToken,
        refreshToken: newRefreshToken,
        accessTokenExpiration: accessTokenExp,
        permissions: decoded.permissions,
        roles: decoded.roles,
        user: {
          id: decoded.id,
          email: decoded.email,
        } as UserDto,
      };
    } catch (err: any) {
      return thunkAPI.rejectWithValue("Failed to refresh token");
    }
  }
);

export const loadTokensFromStorage = createAsyncThunk(
  "auth/loadTokensFromStorage",
  async (_, thunkAPI) => {
    try {
      const [accessToken, refreshToken] = await AsyncStorage.multiGet([
        "accessToken",
        "refreshToken",
      ]).then((results) => results.map((item) => item[1]));

      if (!accessToken || !refreshToken) {
        return thunkAPI.rejectWithValue("No stored tokens");
      }

      const decoded: DecodedToken = jwtDecode(accessToken);
      const accessTokenExpiration = new Date(decoded.exp * 1000);

      return {
        accessToken,
        refreshToken,
        accessTokenExpiration,
        permissions: decoded.permissions,
        roles: decoded.roles,
        user: {
          id: decoded.id,
          email: decoded.email,
        } as UserDto,
      };
    } catch {
      return thunkAPI.rejectWithValue("Failed to load stored tokens");
    }
  }
);

export const fetchUserInfo = createAsyncThunk(
  "auth/fetchUserInfo",
  async (_, thunkAPI) => {
    try {
      const response = await accountService.GetProfile();
      return {
        user: response.data,
      };
    } catch (err: any) {
      return thunkAPI.rejectWithValue("Failed to fetch user info");
    }
  }
);

export const logout = createAsyncThunk("auth/logout", async () => {
  const refreshToken = await AsyncStorage.getItem("refreshToken");

  if (refreshToken) {
    await authService.revoke(refreshToken);
  }

  await AsyncStorage.multiRemove(["accessToken", "refreshToken"]);
});

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder

      //login
      .addCase(login.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(login.fulfilled, (state, action) => {
        state.loading = false;
        state.isAuthenticated = true;
        state.accessToken = action.payload.accessToken;
        state.refreshToken = action.payload.refreshToken;
        state.accessTokenExpiration = action.payload.accessTokenExpiration;
        state.roles = action.payload.roles;
        state.permissions = action.payload.permissions;
        state.user = action.payload.user;
      })
      .addCase(login.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })

      // Refresh
      .addCase(refreshToken.fulfilled, (state, action) => {
        state.isAuthenticated = true;
        state.accessToken = action.payload.accessToken;
        state.refreshToken = action.payload.refreshToken;
        state.accessTokenExpiration = action.payload.accessTokenExpiration;
        state.roles = action.payload.roles;
        state.permissions = action.payload.permissions;
        state.user = action.payload.user;
      })
      .addCase(refreshToken.rejected, (state) => {
        Object.assign(state, initialState);
      })

      // Load from storage
      .addCase(loadTokensFromStorage.fulfilled, (state, action) => {
        state.isAuthenticated = true;
        state.accessToken = action.payload.accessToken;
        state.refreshToken = action.payload.refreshToken;
        state.accessTokenExpiration = action.payload.accessTokenExpiration;
        state.roles = action.payload.roles;
        state.permissions = action.payload.permissions;
        state.user = action.payload.user;
      })
      .addCase(loadTokensFromStorage.rejected, (state) => {
        Object.assign(state, initialState);
      })

      // Fetch user info
      .addCase(fetchUserInfo.fulfilled, (state, action) => {
        if (state.user) {
          state.user = { ...state.user, ...action.payload.user };
        }
      })

      // Logout
      .addCase(logout.fulfilled, (state) => {
        Object.assign(state, initialState);
      });
  },
});

export default authSlice.reducer;
