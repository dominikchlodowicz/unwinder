import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OfferRecommendationsComponent } from '../offer-recommendations/offer-recommendations.component';

@Component({
  selector: 'app-main-homepage-content',
  standalone: true,
  imports: [CommonModule, OfferRecommendationsComponent],
  templateUrl: './main-homepage-content.component.html',
  styleUrl: './main-homepage-content.component.scss',
})
export class MainHomepageContentComponent {}
