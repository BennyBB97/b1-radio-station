import { Routes } from '@angular/router';
import { TrackEditorComponent } from './track-editor/track-editor.component';
import { TrackListComponent } from './track-list/track-list.component';
import { TrackCreatorComponent } from './track-creator/track-creator.component';

export const routes: Routes = [
    { path: '', component: TrackListComponent },
    { path: "track/:id", component: TrackEditorComponent},
    { path: "create/track", component: TrackCreatorComponent}
];
