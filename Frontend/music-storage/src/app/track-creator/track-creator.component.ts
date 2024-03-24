import { Component } from '@angular/core';
import { MusicStorageService } from '../services/music-storage.service';
import { CommonModule, NgFor } from '@angular/common';
import { FormArray, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { FormBuilder } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { Genre } from '../model/genre';
import { CreateTrack } from '../model/createTrack';
import { Router } from '@angular/router';

@Component({
  selector: 'app-track-creator',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, NgFor, MatIconModule, MatButtonModule, CommonModule],
  templateUrl: './track-creator.component.html',
  styleUrl: './track-creator.component.css'
})
export class TrackCreatorComponent {
  
  newTrackForm!: FormGroup;
  genres: Genre[] = [];
  
  constructor (private musicService: MusicStorageService, private fb: FormBuilder, private router: Router){  
  }

  ngOnInit(): void {
    this.newTrackForm = this.fb.group({
      titel: ['', Validators.required],
      genreId: ['', Validators.required],
      artists: this.fb.array([this.fb.control('', Validators.required)])
    })
    this.musicService.getGenres().subscribe(resp => this.genres = resp);
  }

  get artists() {
    return this.newTrackForm.get('artists') as FormArray;
  }

  addArtist(){
    this.artists.push(this.fb.control('', Validators.required));
  }

  deleteArtist(index: number){
    if(this.artists.length > 1){
      this.artists.removeAt(index);
    }
    
  }

  

  onSubmit(){    
    let createTrack = {} as CreateTrack;
    createTrack.titel = this.newTrackForm.get('titel')?.value ?? '';
    createTrack.genreId = this.newTrackForm.get('genreId')?.value ?? 0;
    let artistList: string[] = [];

    for (let i = 0; i < this.artists.length; i++) {
      artistList.push(this.artists.at(i).value);
    }
    createTrack.artistNames = artistList;
    this.musicService.createTrack(createTrack).subscribe();
    this.router.navigate(['/'])
    .then(() => {      
      window.location.reload();
    });
  }
}
