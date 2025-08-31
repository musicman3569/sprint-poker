/**
 * Type helper that converts a type to Date if it's compatible (string, number, or Date),
 * otherwise returns never.
 */
type ToDate<T> = T extends string | number | Date ? Date : never;


/**
 * Converts specified fields in an array of objects from string/number/Date to Date objects.
 *
 * @typeParam T - The type of objects in the input array. Must be a record with string keys.
 * @typeParam K - The type of keys to be converted to dates. Must be valid keys of T.
 *
 * @param data - An array of objects to process. The array is readonly to prevent mutations.
 * @param keys - An array of keys whose values should be converted to Date objects.
 *
 * @returns A new array where specified fields are converted to Date objects.
 *          The type system ensures that only specified keys are converted to Date
 *          while other fields retain their original types.
 *
 * @example
 * const data = [
 *   { id: 1, created: "2025-08-15", name: "Example" }
 * ];
 * const result = convertStringDates(data, ["created"]);
 * // result[0].created will be a Date object
 */
export function convertStringDates<
  T extends Record<string, unknown>,
  K extends keyof T
>(
  data: readonly T[],
  keys: readonly K[]
): Array<{ [P in keyof T]: P extends K ? ToDate<T[P]> : T[P] }> {
  return data.map(item => {
    // Create a shallow copy of the item to avoid mutating the original
    const result: any = {...item};
    // Iterate through the specified keys and convert their values to Date objects
    for (const k of keys) {
      const v = item[k];
      // Only convert non-null values to avoid creating invalid dates
      if (v != null) {
        result[k] = new Date(v as string | number | Date);
      }
    }
    return result;
  });
}
