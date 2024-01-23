import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DynamicProviderSwitchService {
  private providerKey = new BehaviorSubject<string>('short');

  setProviderKey(key: string): void {
    this.providerKey.next(key);
  }

  getCurrentProviderKey(): string {
    return this.providerKey.getValue();
  }
}

export function dynamicProviderFactory(
  switchService: DynamicProviderSwitchService,
  providersMap: { [key: string]: any },
) {
  return () => {
    const key = switchService.getCurrentProviderKey(); // Synchronous method
    return new providersMap[key]();
  };
}
