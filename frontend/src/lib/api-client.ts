import { SkiField } from "@/types/ski-field";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5127";

export interface QuerySkiFieldsParams {
  skip?: number;
  take?: number;
  name?: string;
  countryCode?: string;
  region?: string;
  currency?: string;
  nearestTown?: string;
  fullDayPassPrice?: {
    min?: number;
    max?: number;
  };
  orderBy?: string;
  orderDirection?: "asc" | "desc";
}

export interface QuerySkiFieldsResponse {
  items: SkiField[];
  totalCount: number;
}

interface FindManyRequest {
  skip?: number;
  take?: number;
  where?: {
    name?: { contains?: string; equals?: string; mode?: "insensitive" | "sensitive" };
    countryCode?: { contains?: string; equals?: string; mode?: "insensitive" | "sensitive" };
    region?: { contains?: string; equals?: string; mode?: "insensitive" | "sensitive" };
    currency?: { contains?: string; equals?: string; mode?: "insensitive" | "sensitive" };
    nearestTown?: { contains?: string; equals?: string; mode?: "insensitive" | "sensitive" };
    fullDayPassPrice?: {
      equals?: number;
      gt?: number;
      gte?: number;
      lt?: number;
      lte?: number;
    };
  };
  orderBy?: Array<{
    field: string;
    direction: "asc" | "desc";
  }>;
}

export const apiClient = {
  async querySkiFields(params: QuerySkiFieldsParams = {}): Promise<QuerySkiFieldsResponse> {
    const requestBody: FindManyRequest = {};

    if (params.skip !== undefined) {
      requestBody.skip = params.skip;
    }
    if (params.take !== undefined) {
      requestBody.take = params.take;
    }

    // Build where filter object
    const where: FindManyRequest["where"] = {};
    if (params.name) {
      where.name = { contains: params.name, mode: "insensitive" };
    }
    if (params.countryCode) {
      where.countryCode = { equals: params.countryCode, mode: "insensitive" };
    }
    if (params.region) {
      where.region = { contains: params.region, mode: "insensitive" };
    }
    if (params.currency) {
      where.currency = { equals: params.currency, mode: "insensitive" };
    }
    if (params.nearestTown) {
      where.nearestTown = { contains: params.nearestTown, mode: "insensitive" };
    }
    if (params.fullDayPassPrice?.min !== undefined || params.fullDayPassPrice?.max !== undefined) {
      where.fullDayPassPrice = {};
      if (params.fullDayPassPrice.min !== undefined) {
        where.fullDayPassPrice.gte = params.fullDayPassPrice.min;
      }
      if (params.fullDayPassPrice.max !== undefined) {
        where.fullDayPassPrice.lte = params.fullDayPassPrice.max;
      }
    }

    if (Object.keys(where).length > 0) {
      requestBody.where = where;
    }

    // Build orderBy
    if (params.orderBy) {
      requestBody.orderBy = [
        {
          field: params.orderBy,
          direction: (params.orderDirection ?? "asc") as "asc" | "desc",
        },
      ];
    }

    const url = `${API_BASE_URL}/api/skifields/query`;
    
    const response = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(requestBody),
    });

    if (!response.ok) {
      throw new Error(`API error: ${response.status} ${response.statusText}`);
    }

    const data = await response.json();
    return data;
  },
};

