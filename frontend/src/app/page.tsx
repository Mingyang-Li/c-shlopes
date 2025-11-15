"use client";

import { useState } from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { useSkiFields } from "@/hooks/use-ski-fields";
import { ChevronLeft, ChevronRight } from "lucide-react";

const ITEMS_PER_PAGE = 10;

export default function HomePage() {
  const [currentPage, setCurrentPage] = useState(1);
  const skip = (currentPage - 1) * ITEMS_PER_PAGE;

  const { data, isLoading, isError, error } = useSkiFields({
    skip,
    take: ITEMS_PER_PAGE,
  });

  const totalCount = data?.totalCount ?? 0;
  const items = data?.items ?? [];
  const totalPages = Math.ceil(totalCount / ITEMS_PER_PAGE);

  const startItem = skip + 1;
  const endItem = Math.min(skip + ITEMS_PER_PAGE, totalCount);

  const handlePreviousPage = () => {
    if (currentPage > 1) {
      setCurrentPage(currentPage - 1);
    }
  };

  const handleNextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage(currentPage + 1);
    }
  };

  return (
    <div className="container mx-auto py-8 px-4 max-w-7xl">
      <div className="mb-8">
        <h1 className="text-4xl font-bold mb-2">Ski Radar</h1>
        <p className="text-muted-foreground">
          Discover and compare ski fields around the world
        </p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Ski Fields</CardTitle>
          <CardDescription>
            Browse and compare ski fields from around the world
          </CardDescription>
        </CardHeader>
        <CardContent>
          {isLoading && (
            <div className="py-8 text-center text-muted-foreground">
              Loading ski fields...
            </div>
          )}

          {isError && (
            <div className="py-8 text-center text-destructive">
              Error loading ski fields: {error?.message ?? "Unknown error"}
            </div>
          )}

          {!isLoading && !isError && (
            <>
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Name</TableHead>
                    <TableHead>Country</TableHead>
                    <TableHead>Region</TableHead>
                    <TableHead className="text-right">Full Day Pass</TableHead>
                    <TableHead>Nearest Town</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {items.length === 0 ? (
                    <TableRow>
                      <TableCell colSpan={5} className="text-center text-muted-foreground">
                        No ski fields found
                      </TableCell>
                    </TableRow>
                  ) : (
                    items.map((field) => (
                      <TableRow key={field.id}>
                        <TableCell className="font-medium">{field.name}</TableCell>
                        <TableCell>{field.countryCode}</TableCell>
                        <TableCell>{field.region}</TableCell>
                        <TableCell className="text-right">
                          {new Intl.NumberFormat("en-US", {
                            style: "currency",
                            currency: field.currency,
                          }).format(field.fullDayPassPrice)}
                        </TableCell>
                        <TableCell>{field.nearestTown}</TableCell>
                      </TableRow>
                    ))
                  )}
                </TableBody>
              </Table>

              {totalCount > 0 && (
                <div className="mt-4 flex items-center justify-between">
                  <div className="text-sm text-muted-foreground">
                    Showing {startItem}-{endItem} of {totalCount} rows
                  </div>
                  <div className="flex items-center gap-2">
                    <Button
                      variant="outline"
                      size="sm"
                      onClick={handlePreviousPage}
                      disabled={currentPage === 1 || isLoading}
                    >
                      <ChevronLeft className="h-4 w-4 mr-1" />
                      Previous
                    </Button>
                    <div className="text-sm text-muted-foreground">
                      Page {currentPage} of {totalPages}
                    </div>
                    <Button
                      variant="outline"
                      size="sm"
                      onClick={handleNextPage}
                      disabled={currentPage >= totalPages || isLoading}
                    >
                      Next
                      <ChevronRight className="h-4 w-4 ml-1" />
                    </Button>
                  </div>
                </div>
              )}
            </>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
