import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { SkiField } from "@/types/ski-field";

// Hardcoded data for initial development
const mockSkiFields: SkiField[] = [
  {
    id: "abc123",
    name: "Whistler Blackcomb",
    countryCode: "CAN",
    region: "British Columbia",
    fullDayPassPrice: 185.0,
    currency: "CAD",
    nearestTown: "Whistler",
    createdAt: "2024-01-01T00:00:00Z",
    updatedAt: "2024-01-01T00:00:00Z",
  },
  {
    id: "def456",
    name: "Vail",
    countryCode: "USA",
    region: "Colorado",
    fullDayPassPrice: 199.0,
    currency: "USD",
    nearestTown: "Vail",
    createdAt: "2024-01-01T00:00:00Z",
    updatedAt: "2024-01-01T00:00:00Z",
  },
  {
    id: "ghi789",
    name: "Niseko",
    countryCode: "JPN",
    region: "Hokkaido",
    fullDayPassPrice: 8500.0,
    currency: "JPY",
    nearestTown: "Niseko",
    createdAt: "2024-01-01T00:00:00Z",
    updatedAt: "2024-01-01T00:00:00Z",
  },
  {
    id: "jkl012",
    name: "Aspen Snowmass",
    countryCode: "USA",
    region: "Colorado",
    fullDayPassPrice: 209.0,
    currency: "USD",
    nearestTown: "Aspen",
    createdAt: "2024-01-01T00:00:00Z",
    updatedAt: "2024-01-01T00:00:00Z",
  },
  {
    id: "mno345",
    name: "Zermatt",
    countryCode: "CHE",
    region: "Valais",
    fullDayPassPrice: 88.0,
    currency: "CHF",
    nearestTown: "Zermatt",
    createdAt: "2024-01-01T00:00:00Z",
    updatedAt: "2024-01-01T00:00:00Z",
  },
];

export default function HomePage() {
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
              {mockSkiFields.map((field) => (
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
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}
