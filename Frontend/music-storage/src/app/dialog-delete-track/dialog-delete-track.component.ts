import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialog,
  MatDialogRef,
  MatDialogActions,
  MatDialogClose,
  MatDialogTitle,
  MatDialogContent,
  MatDialogModule,
} from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-delete-track',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule],
  templateUrl: './dialog-delete-track.component.html',
  styleUrl: './dialog-delete-track.component.css'
})
export class DialogDeleteTrackComponent {
  constructor(private dialogRef: MatDialogRef<DialogDeleteTrackComponent>) { 

  }
}
