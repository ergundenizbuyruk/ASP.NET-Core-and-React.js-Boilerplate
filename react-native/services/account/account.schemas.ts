import { z } from "zod";

export const createUserSchema = z.object({
  email: z.email("Geçerli bir e-posta girin"),
  password: z
    .string("Parola giriniz")
    .min(6, "Parola en az 6 karakter olmalıdır"),
  firstName: z.string("Ad giriniz").min(1, "Ad giriniz"),
  lastName: z.string("Soyad giriniz").min(1, "Soyad giriniz"),
  phoneNumber: z
    .string("Telefon numarası giriniz")
    .regex(/^[0-9]{10,15}$/, "Geçerli bir telefon numarası girin"),
});

export const updateProfileSchema = z.object({
  firstName: z.string("Ad giriniz").min(1, "Ad giriniz"),
  lastName: z.string("Soyad giriniz").min(1, "Soyad giriniz"),
});

export const resetPasswordSchema = z
  .object({
    email: z.email("Geçerli bir e-posta girin"),
    token: z
      .string("Geçerli bir kod girin")
      .length(6, "Kod 6 karakter olmalıdır"),
    newPassword: z
      .string("Parola giriniz")
      .min(6, "Parola en az 6 karakter olmalıdır"),
    confirmPassword: z
      .string("Parola tekrarı giriniz")
      .min(6, "Parola tekrarı en az 6 karakter olmalıdır"),
  })
  .refine((data) => data.newPassword === data.confirmPassword, {
    message: "Parolalar eşleşmiyor",
    path: ["confirmPassword"],
  });

export type CreateUserDto = z.infer<typeof createUserSchema>;
export type UpdateProfileDto = z.infer<typeof updateProfileSchema>;
export type ResetPasswordDto = z.infer<typeof resetPasswordSchema>;
