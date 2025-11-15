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
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { SkiField } from "@/types/ski-field";
import { Trash2, Plus } from "lucide-react";

// Hardcoded data for admin panel
const initialSkiFields: SkiField[] = [
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
];

export default function AdminPage() {
  const [skiFields, setSkiFields] = useState<SkiField[]>(initialSkiFields);
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);
  const [formData, setFormData] = useState({
    name: "",
    countryCode: "",
    region: "",
    fullDayPassPrice: "",
    currency: "",
    nearestTown: "",
  });

  const handleCreate = () => {
    const newField: SkiField = {
      id: Math.random().toString(36).substring(7),
      name: formData.name,
      countryCode: formData.countryCode.toUpperCase(),
      region: formData.region,
      fullDayPassPrice: parseFloat(formData.fullDayPassPrice),
      currency: formData.currency.toUpperCase(),
      nearestTown: formData.nearestTown,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
    };

    setSkiFields([...skiFields, newField]);
    setFormData({
      name: "",
      countryCode: "",
      region: "",
      fullDayPassPrice: "",
      currency: "",
      nearestTown: "",
    });
    setIsCreateDialogOpen(false);
  };

  const handleDelete = (id: string) => {
    setSkiFields(skiFields.filter((field) => field.id !== id));
  };

  return (
    <div className="container mx-auto py-8 px-4 max-w-7xl">
      <div className="mb-8 flex justify-between items-center">
        <div>
          <h1 className="text-4xl font-bold mb-2">Admin Panel</h1>
          <p className="text-muted-foreground">
            Manage ski fields database
          </p>
        </div>
        <Dialog open={isCreateDialogOpen} onOpenChange={setIsCreateDialogOpen}>
          <DialogTrigger asChild>
            <Button>
              <Plus className="mr-2 h-4 w-4" />
              Add Ski Field
            </Button>
          </DialogTrigger>
          <DialogContent className="sm:max-w-[425px]">
            <DialogHeader>
              <DialogTitle>Add New Ski Field</DialogTitle>
              <DialogDescription>
                Enter the details for the new ski field.
              </DialogDescription>
            </DialogHeader>
            <div className="grid gap-4 py-4">
              <div className="grid gap-2">
                <Label htmlFor="name">Name</Label>
                <Input
                  id="name"
                  value={formData.name}
                  onChange={(e) =>
                    setFormData({ ...formData, name: e.target.value })
                  }
                  placeholder="Whistler Blackcomb"
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="countryCode">Country Code</Label>
                <Input
                  id="countryCode"
                  value={formData.countryCode}
                  onChange={(e) =>
                    setFormData({ ...formData, countryCode: e.target.value })
                  }
                  placeholder="CAN"
                  maxLength={3}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="region">Region</Label>
                <Input
                  id="region"
                  value={formData.region}
                  onChange={(e) =>
                    setFormData({ ...formData, region: e.target.value })
                  }
                  placeholder="British Columbia"
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="fullDayPassPrice">Full Day Pass Price</Label>
                <Input
                  id="fullDayPassPrice"
                  type="number"
                  step="0.01"
                  value={formData.fullDayPassPrice}
                  onChange={(e) =>
                    setFormData({ ...formData, fullDayPassPrice: e.target.value })
                  }
                  placeholder="185.00"
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="currency">Currency</Label>
                <Input
                  id="currency"
                  value={formData.currency}
                  onChange={(e) =>
                    setFormData({ ...formData, currency: e.target.value })
                  }
                  placeholder="CAD"
                  maxLength={3}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="nearestTown">Nearest Town</Label>
                <Input
                  id="nearestTown"
                  value={formData.nearestTown}
                  onChange={(e) =>
                    setFormData({ ...formData, nearestTown: e.target.value })
                  }
                  placeholder="Whistler"
                />
              </div>
            </div>
            <DialogFooter>
              <Button
                variant="outline"
                onClick={() => setIsCreateDialogOpen(false)}
              >
                Cancel
              </Button>
              <Button onClick={handleCreate}>Create</Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Ski Fields</CardTitle>
          <CardDescription>
            View and manage all ski fields in the database
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
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {skiFields.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={6} className="text-center text-muted-foreground">
                    No ski fields found
                  </TableCell>
                </TableRow>
              ) : (
                skiFields.map((field) => (
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
                    <TableCell className="text-right">
                      <Button
                        variant="destructive"
                        size="sm"
                        onClick={() => handleDelete(field.id)}
                      >
                        <Trash2 className="h-4 w-4" />
                      </Button>
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}

