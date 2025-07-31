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

export type CreateUserDto = z.infer<typeof createUserSchema>;
export type UpdateProfileDto = z.infer<typeof updateProfileSchema>;
