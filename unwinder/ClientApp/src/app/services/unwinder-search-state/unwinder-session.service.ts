import { Injectable } from '@angular/core';
import { UnwinderSessionProperty } from '../../interfaces/flight-data-exchange/unwinder-session-property';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UnwinderSessionService {
  private sessionData =
    new BehaviorSubject<Partial<UnwinderSessionProperty> | null>(null);

  setData(property: keyof UnwinderSessionProperty, data: any): Observable<any> {
    return new Observable((observer) => {
      const currentData = this.sessionData.value || {};
      currentData[property] = data;
      this.sessionData.next(currentData);
      observer.next(currentData);
      observer.complete();
    });
  }

  getData(property: keyof UnwinderSessionProperty): any {
    const currentData = this.sessionData.value;
    if (!currentData) {
      console.warn('Session data is not initialized');
      return null;
    }
    return currentData[property];
  }

  getSessionDataObservable(): Observable<Partial<UnwinderSessionProperty> | null> {
    return this.sessionData.asObservable();
  }

  clearData(property: keyof UnwinderSessionProperty) {
    const currentData = this.sessionData.value || {}; // Fallback to an empty object if null
    if (property in currentData) {
      delete currentData[property]; // Use delete to remove the property
      this.sessionData.next(currentData);
    } else {
      throw new Error(
        `Property '${property}' does not exist on UnwinderSessionProperty`,
      );
    }
  }
}
