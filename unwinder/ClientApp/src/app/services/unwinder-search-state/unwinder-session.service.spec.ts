import { UnwinderSessionService } from './unwinder-session.service';
import { UnwinderSessionProperty } from '../../interfaces/flight-data-exchange/unwinder-session-property';

describe('UnwinderSessionService', () => {
  let service: UnwinderSessionService;

  beforeEach(() => {
    service = new UnwinderSessionService();
  });

  it('should set data and emit the updated value', (done) => {
    const testProperty: keyof UnwinderSessionProperty = 'flightParameters';
    const testData = {
      where: 'NYC',
      origin: 'LAX',
      when: new Date(),
      numberOfPassengers: 1,
    };

    service.setData(testProperty, testData).subscribe((data) => {
      expect(data[testProperty]).toEqual(testData);
      done();
    });
  });

  it('should get the correct data for a property', () => {
    const testProperty: keyof UnwinderSessionProperty = 'flightParameters';
    const testData = {
      where: 'NYC',
      origin: 'LAX',
      when: new Date(),
      numberOfPassengers: 1,
    };

    service.setData(testProperty, testData).subscribe(() => {
      const result = service.getData(testProperty);
      expect(result).toEqual(testData);
    });
  });

  it('should emit the current session data through getSessionDataObservable', (done) => {
    const testProperty: keyof UnwinderSessionProperty = 'flightParameters';
    const testData = {
      where: 'NYC',
      origin: 'LAX',
      when: new Date(),
      numberOfPassengers: 1,
    };

    service.setData(testProperty, testData).subscribe(() => {
      service.getSessionDataObservable().subscribe((data) => {
        expect(data![testProperty]).toEqual(testData);
        done();
      });
    });
  });

  it('should clear the specified data property', () => {
    const testProperty: keyof UnwinderSessionProperty = 'flightParameters';
    const testData = {
      where: 'NYC',
      origin: 'LAX',
      when: new Date(),
      numberOfPassengers: 1,
    };

    service.setData(testProperty, testData).subscribe(() => {
      service.clearData(testProperty);

      const result = service.getData(testProperty);
      expect(result).toBeUndefined();
    });
  });

  it('should throw an error if trying to clear a non-existent property', () => {
    const testProperty = 'nonExistentProperty' as keyof UnwinderSessionProperty;

    expect(() => service.clearData(testProperty)).toThrowError(
      `Property '${testProperty}' does not exist on UnwinderSessionProperty`,
    );
  });
});
