import { Gift } from "./gift.model";

export interface Donor {
    id: number;
   firstName: string;
    lastName: string;
    eMail: string;
    gifts?: Gift[];
}

export interface AddDonorDto {
   firstName: string;
    lastName: string;
    eMail: string;
}

