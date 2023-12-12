import { Component, Injectable } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DateRange, MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
import { DateAdapter, MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-which-weekend',
  standalone: true,
  imports: [CommonModule, MatDatepickerModule, 
    MatInputModule, ReactiveFormsModule, MatNativeDateModule],
  templateUrl: './which-weekend.component.html',
  styleUrl: './which-weekend.component.css',
})
export class WhichWeekendComponent {
  weekendFilter = (d: Date | null): boolean => {
    const day = (d || new Date()).getDay();
    // Allow only Saturday (6) and Sunday (0)
    return day === 0 || day === 6;
  };

  //TODO: Add selector only for weekends
}
