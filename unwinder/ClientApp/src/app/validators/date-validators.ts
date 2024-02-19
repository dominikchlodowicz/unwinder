import { FormControl } from '@angular/forms';

export function dateNotInThePast(
  control: FormControl,
): { [key: string]: any } | null {
  const chosenDate = control.value;
  if (!chosenDate) {
    return null;
  }

  const currentDate = new Date();
  currentDate.setHours(0, 0, 0, 0);

  if (chosenDate < currentDate) {
    return { dateNotInThePast: true };
  }

  return null;
}
