import { Person } from './person.model';

export class User extends Person {
    id: number
    email: string;
    memberType: string;
    status: string;
}