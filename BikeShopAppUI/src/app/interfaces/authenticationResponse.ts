import { User } from "./user";

export interface AuthenticationResponse
{
    user: User | null;
    token: string | null;
    refreshToken: string | null;
    refreshTokenExpirationDateTime: Date | null;
    isAdmin: boolean | null;
}