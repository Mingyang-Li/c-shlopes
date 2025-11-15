import { z } from "zod";

export const skiFieldSchema = z.object({
  id: z.string(),
  name: z.string(),
  countryCode: z.string().length(3),
  region: z.string(),
  fullDayPassPrice: z.number(),
  currency: z.string().length(3),
  nearestTown: z.string(),
  createdAt: z.string(),
  updatedAt: z.string(),
});

export type SkiField = z.infer<typeof skiFieldSchema>;

export const paginatedSkiFieldsSchema = z.object({
  items: z.array(skiFieldSchema),
  totalCount: z.number(),
});

export type PaginatedSkiFields = z.infer<typeof paginatedSkiFieldsSchema>;

export const createSkiFieldSchema = z.object({
  name: z.string().min(1, "Name is required"),
  countryCode: z.string().length(3, "Country code must be 3 characters"),
  region: z.string().min(1, "Region is required"),
  fullDayPassPrice: z.number().positive("Price must be positive"),
  currency: z.string().length(3, "Currency must be 3 characters"),
  nearestTown: z.string().min(1, "Nearest town is required"),
});

export type CreateSkiFieldInput = z.infer<typeof createSkiFieldSchema>;

