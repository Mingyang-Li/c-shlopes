import { useQuery } from "@tanstack/react-query";
import { apiClient, QuerySkiFieldsParams } from "@/lib/api-client";
import { QuerySkiFieldsResponse } from "@/lib/api-client";

export function useSkiFields(params: QuerySkiFieldsParams = {}) {
  const { skip = 0, take = 10, ...restParams } = params;

  return useQuery<QuerySkiFieldsResponse, Error>({
    queryKey: ["skiFields", { skip, take, ...restParams }],
    queryFn: () => apiClient.querySkiFields({ skip, take, ...restParams }),
    staleTime: 5 * 60 * 1000, // Consider data fresh for 5 minutes
    gcTime: 30 * 60 * 1000, // Keep in cache for 30 minutes (formerly cacheTime)
  });
}

