import { Artist } from "./artist";
import { Genre } from "./genre";

export interface Track{
    trackId: number;
    titel: string;
    genre: Genre 
    artists: Artist[];
}

