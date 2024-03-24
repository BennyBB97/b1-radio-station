import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogDeleteTrackComponent } from './dialog-delete-track.component';

describe('DialogDeleteTrackComponent', () => {
  let component: DialogDeleteTrackComponent;
  let fixture: ComponentFixture<DialogDeleteTrackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DialogDeleteTrackComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DialogDeleteTrackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
