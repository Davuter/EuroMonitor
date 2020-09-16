export interface ICatalogItem {
  id: number;
  name: string;
  description: string;
  price: number;
  pictureUri: string;
  catalogOwnerId: number;
  subscriptedUser: boolean;
  subscriptedId: number;
}
