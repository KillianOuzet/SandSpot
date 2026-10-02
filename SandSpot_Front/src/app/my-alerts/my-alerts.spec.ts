import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MyAlerts } from './my-alerts';

describe('MyAlerts', () => {
  let component: MyAlerts;
  let fixture: ComponentFixture<MyAlerts>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyAlerts],
    }).compileComponents();

    fixture = TestBed.createComponent(MyAlerts);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
