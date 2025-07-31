import axios from "axios";

const apiClient = axios.create({
  baseURL: process.env.EXPO_PUBLIC_API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export default apiClient;
