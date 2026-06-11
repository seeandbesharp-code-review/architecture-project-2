import { AuthResponse } from "./user.model";

export interface Winner {
  idGift: number;
  idUser: number;
  user:AuthResponse;
}