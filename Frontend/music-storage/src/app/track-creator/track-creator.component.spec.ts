import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrackCreatorComponent } from './track-creator.component';

describe('TrackCreatorComponent', () => {
  let component: TrackCreatorComponent;
  let fixture: ComponentFixture<TrackCreatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrackCreatorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TrackCreatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
