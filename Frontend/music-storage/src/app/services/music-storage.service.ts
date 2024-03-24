import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Track } from '../model/track';
import { Genre } from '../model/genre';
import { CreateTrack } from '../model/createTrack';

@Injectable({
  providedIn: 'root'
})
export class MusicStorageService {

  readonly ROOT_URL = 'https://localhost:7192/api/Music';

  constructor(private http: HttpClient) { }


  getTracks(){
    return this.http.get<Track[]>(this.ROOT_URL + '/tracks');
  }

  getTrack(trackId: number){
    return this.http.get<Track>(this.ROOT_URL + '/track/' + trackId);
  }

  getGenres(){
    return this.http.get<Genre[]>(this.ROOT_URL + '/genres');
  }

  searchTracks(searchTerm: string, type: number){
    return this.http.get<Track[]>(this.ROOT_URL + '/track/' + searchTerm + '/' + type);
  }

  createTrack(createTrack: CreateTrack){
    return this.http.post(this.ROOT_URL + '/track', createTrack);
  }

  deleteTrack(trackId: number){
    return this.http.delete(this.ROOT_URL + '/track/' + trackId);
  }

}
