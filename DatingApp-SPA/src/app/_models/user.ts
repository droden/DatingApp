import {Photo} from './photo';

export interface User {
    id: number;
    username: string;
    knownAs: string;
    gender: string;
    age: string;
    created: Date;
    lastActive: Date;
    photoURL: string;
    city: string;
    country: string;

    interests?: string;
    introMessage?: string;
    lookingForMessage?: string;
    photos?: Photo[];
}
