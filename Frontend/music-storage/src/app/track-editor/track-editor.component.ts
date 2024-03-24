import { Component, OnInit } from '@angular/core';
import { MusicStorageService } from '../services/music-storage.service';
import { Track } from '../model/track';
import { ActivatedRoute } from '@angular/router';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Genre } from '../model/genre';
import {MatSelectModule} from '@angular/material/select';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-track-editor',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, NgFor],
  templateUrl: './track-editor.component.html',
  styleUrl: './track-editor.component.css'
})
export class TrackEditorComponent implements OnInit{
  
  trackId!: number;
  track!: Track;
  genres: Genre[] = [];
  trackForm = new FormGroup({
    trackTitel: new FormControl(''),
    genreId: new FormArray([]),
  })
    
  constructor (private musicService: MusicStorageService, private route: ActivatedRoute){  
  }
  
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.trackId = Number(params.get('id'));
    });    
    this.musicService.getTrack(this.trackId).subscribe(resp => {
      this.track = resp;

      this.trackForm.patchValue({
        trackTitel: this.track.titel
      });
    })   
    
    this.musicService.getGenres().subscribe(resp => {
      this.genres = resp;

      this.trackForm.patchValue({
       
      });
    })
  }

 
  editTrackSubmit(){    
    let term: string;
    
  }
}
