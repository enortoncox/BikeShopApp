export interface CustomError
{
    status: number;
    detail: string;
    title: string;
    errors: any | null;
}