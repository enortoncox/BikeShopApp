import { User } from "./user";

export interface VerifyTokenResponse{
    user: User | null;
    isAdmin: boolean;
    refreshValid: boolean;
    jwtValid: boolean;
}