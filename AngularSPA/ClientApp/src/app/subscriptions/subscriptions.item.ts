import { Status } from "./subscriptions.status";

export interface Subscriptions {
  id: number;
  subscritionDate: string;
  productId: number;
  productName: string;
  status: Status;
  unSubscritionDate: string;
}


