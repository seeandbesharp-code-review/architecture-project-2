

export interface AddUserDto {
   FirstName: string;
    LastName: string;
    Email: string;
    Password: string;
    PhoneNumber?: string;
}

export interface LogInUserDto {
    Email: string;
    Password: string;
}

export interface AuthResponse {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber?: string; 
    role?: string;
    token: string; 
}

export interface Winner {
  id: number;
  idUser: number;
  idGift: number;
  user?: AuthResponse;
  gift?: { id: number; name: string; price?: number };
}