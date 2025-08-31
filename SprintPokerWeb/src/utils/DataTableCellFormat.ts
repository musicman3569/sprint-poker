/**
 * Represents a generic row data structure where keys are strings and values can be of any type.
 * Used for storing and accessing data in table rows.
 */
export interface RowData {
    [key: string]: any;
}

/**
 * Formats a numeric value from the row data with specified number of decimal places.
 * @param rowData - The data object containing the value to format
 * @param propertyName - The property name to access the value in the row data
 * @param decimals - Maximum number of decimal places to show (default: 0)
 * @returns Formatted number string with specified decimals, or empty string if value is not a number
 */
export const formatNumber = (rowData: RowData, propertyName: string, decimals: number = 0, suffix: string = ''): string => {
    const value = rowData[propertyName];
    return typeof value === 'number' ? value.toLocaleString('en-US', {
        minimumFractionDigits: 0,
        maximumFractionDigits: decimals
    }) + suffix : '';
};

/**
 * Formats a date value from the row data using the local date string format.
 * @param rowData - The data object containing the date to format
 * @param propertyName - The property name to access the date in the row data
 * @returns Formatted date string using local format, or empty string if value is not a Date
 */
export const formatDateAsLocal = (rowData: RowData, propertyName: string): string => {
    const value = rowData[propertyName];
    return value instanceof Date ? value.toLocaleDateString() : '';
};

/**
 * Formats a date value from the row data using a custom format pattern.
 * @param rowData - The data object containing the date to format
 * @param propertyName - The property name to access the date in the row data
 * @param format - Custom format pattern (default: 'yyyy-MM-dd HH:mm:ss')
 * @returns Formatted date string according to specified pattern, or empty string if value is not a Date
 */
export const formatDateCustom = (rowData: RowData, propertyName: string, format: string = 'yyyy-MM-dd HH:mm:ss'): string => {
    const value = rowData[propertyName];
    if (!(value instanceof Date)) return '';
    return format
        .replace('yyyy', value.getFullYear().toString())
        .replace('MM', (value.getMonth() + 1).toString().padStart(2, '0'))
        .replace('dd', value.getDate().toString().padStart(2, '0'))
        .replace('HH', value.getHours().toString().padStart(2, '0'))
        .replace('mm', value.getMinutes().toString().padStart(2, '0'))
        .replace('ss', value.getSeconds().toString().padStart(2, '0'))
        ;
};

/**
 * Formats a numeric value from the row data as a percentage with specified decimal places.
 * @param rowData - The data object containing the value to format
 * @param propertyName - The property name to access the value in the row data
 * @param decimals - Number of decimal places to show (default: 1)
 * @returns Formatted percentage string with % symbol, or empty string if value is not a number
 */
export const formatPercentage = (rowData: RowData, propertyName: string, decimals: number = 1): string => {
    const value = rowData[propertyName];
    return typeof value === 'number' ? `${value.toFixed(decimals)}%` : '';
};

/**
 * Formats a numeric value from the row data as currency with specified currency code.
 * @param rowData - The data object containing the value to format
 * @param propertyName - The property name to access the value in the row data
 * @param currency - ISO currency code (default: 'USD')
 * @returns Formatted currency string using locale formatting, or empty string if value is not a number
 */
export const formatCurrency = (rowData: RowData, propertyName: string, currency: string = 'USD'): string => {
    const value = rowData[propertyName];
    return typeof value === 'number'
        ? value.toLocaleString('en-US', {style: 'currency', currency: currency})
        : '';
};


/**
 * Formats a field name by converting snake_case to Title Case format.
 * @param field - The field name in snake_case format to be converted
 * @returns Formatted string with each word capitalized and spaces between words
 */
export const formatHeaderText = (field: string): string => {
    return field
        .split('_')
        .map(word => word
            .charAt(0)
            .toUpperCase() + word.slice(1)
        )
        .join(' ');
};