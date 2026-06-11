import { Gift } from "./gift.model";

export interface GetBagDto {
    id: number;
    idUser: number;
    idGift: number;
    gift:Gift;
 quantity: number;
}   
export interface CreateBagDto {
    idUser: number;
    idGift: number;
     quantity: number;
} 