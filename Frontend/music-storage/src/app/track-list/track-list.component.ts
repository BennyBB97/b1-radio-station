import { Component, OnInit } from '@angular/core';
import { Track } from '../model/track';
import { MusicStorageService } from '../services/music-storage.service'
import {MatCardModule} from '@angular/material/card'; 
import { NgFor } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import {MatGridListModule} from '@angular/material/grid-list';
import { RouterModule } from '@angular/router';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatIconModule} from '@angular/material/icon';
import {MatChipsModule} from '@angular/material/chips';
import {MatDialog, MatDialogModule} from '@angular/material/dialog';
import { DialogDeleteTrackComponent } from '../dialog-delete-track/dialog-delete-track.component';
import {MatTableModule} from '@angular/material/table';

@Component({
  selector: 'app-track-list',
  standalone: true,
  imports: [MatTableModule, MatDialogModule, MatCardModule, MatButtonModule, MatGridListModule, NgFor, RouterModule, MatFormFieldModule, MatInputModule, MatIconModule, MatChipsModule, ReactiveFormsModule],
  templateUrl: './track-list.component.html',
  styleUrl: './track-list.component.css'
})

export class TrackListComponent implements OnInit{
  tracks: Track[];
  //searchTerm = '';
  dataSource: any[] = [];
  displayedColumns: string[] = ['titel'];

  searchForm = new FormGroup({
    searchTerm: new FormControl(''),
    type: new FormControl('4'),
  });

  constructor (private musicService: MusicStorageService, public dialog: MatDialog){
    this.tracks = [];
  }
  
  ngOnInit(): void {
    this.getAllTracks();
  }

  getAllTracks() {
    this.musicService.getTracks().subscribe(resp => this.tracks = resp);
  }

  onSubmit(){    
    let term: string;
    let type: number;
    term = this.searchForm.get('searchTerm')?.value ?? ''; 
    type = (this.searchForm.get('type')?.value ?? 4) as number; 

    if (term.length === 0) {
      this.getAllTracks();      
    } else {
      this.musicService.searchTracks(term, type).subscribe(resp => this.tracks = resp);
    }    
  }

  clearSearchTerm(){
    this.searchForm.controls['searchTerm'].setValue('');
  }

  openDialogDeleteTrack(trackId: number) {
    let dialogRef = this.dialog.open(DialogDeleteTrackComponent);

    dialogRef.afterClosed().subscribe(result => {
      if(result != false){
        this.musicService.deleteTrack(trackId).subscribe();    
        location.reload();
      }
    }); 
  }  
  
}
