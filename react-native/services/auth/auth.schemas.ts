import { z } from "zod";

export const loginSchema = z.object({
  email: z.email("Geçerli bir e-posta girin"),
  password: z
    .string("Parola giriniz")
    .min(6, "Parola en az 6 karakter olmalıdır"),
});

export type LoginDto = z.infer<typeof loginSchema>;
