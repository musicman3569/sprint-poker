import { FilterMatchMode, FilterOperator } from 'primereact/api';
import { type DataTableFilterMeta } from 'primereact/datatable';
import type { SelectItem } from "primereact/selectitem";

/**
 * Defines the available filter types for DataTable columns.
 * Each type corresponds to a specific filtering behavior and UI component.
 */
export type ColumnFilterKind =
    | 'id'
    | 'text'
    | 'number'
    | 'date'
    | 'dropdown'
    | 'multiselect'
    | 'boolean';

export interface ColumnSpec {
    kind: ColumnFilterKind;
    dataType?: 'text' | 'numeric' | 'date' | string | undefined;
    width?: string;
    frozen?: boolean;
    decimalPlaces?: number;
    /** Override operator if needed (defaults chosen per kind). */
    operator?: FilterOperator;
    /** For simple kinds use a single matchMode override (optional). */
    matchMode?: FilterMatchMode;
    isDataKey?: boolean;
    isDisplayName?: boolean;
    isReadOnly?: boolean;
    isHidden?: boolean;
    displaySuffix?: string;
    selectItems?: SelectItem[];
    selectLabelField?: string;
    selectValueField?: string;
}

export type ModelSpec = Record<string, ColumnSpec>;

/**
 * Retrieves the field name that is marked as the data key in the model specification.
 * The data key is typically used as a unique identifier for records in the data table.
 *
 * @param spec - The model specification containing field definitions and their properties
 * @returns The field name that is marked as the data key
 * @throws Error if no field is marked as a data key in the model specification
 */
export function getModelDataKey(spec: ModelSpec): string {
    for (const [field, def] of Object.entries(spec)) {
        if (def.isDataKey) {
            return field;
        }
    }
    throw new Error('No data key found in model spec');
}

export function getModelDisplayName(spec: ModelSpec): string {
    for (const [field, def] of Object.entries(spec)) {
        if (def.isDisplayName) {
            return field;
        }
    }
    return getModelDataKey(spec);
}

export function getModelSelectItems(columnSpec: ColumnSpec, rowDataFieldValue: any[]): SelectItem[] {
    const items: SelectItem[] = [];
    for (const item of rowDataFieldValue) {
        items.push({
            label: item[columnSpec.selectLabelField ?? 'Name'],
            value: item[columnSpec.selectValueField ?? 'Name']
        } as SelectItem);
    }
    return items;
}


/**
 * Retrieves an array of field names from the model specification that are eligible for global filtering.
 * Only fields with 'id' or 'text' kinds are included in the global filter.
 *
 * @param modelSpec - The model specification containing field definitions and their properties
 * @returns An array of field names that can be used for global filtering
 */
export function getModelGlobalFilterFields(modelSpec: ModelSpec): string[] {
    return Object.entries(modelSpec)
        .filter(([_, def]) => def.kind === 'id' || def.kind === 'text')
        .map(([field]) => field);
}

/**
 * Configuration options for the useTableFilters hook.
 * Controls the behavior and appearance of table filtering functionality.
 */
export interface UseTableFiltersOptions {
    /** Fields to search when typing in the global search input. */
    globalFilterFields?: string[];
    /** Placeholder text for the global search input. */
    globalPlaceholder?: string;
    /** Whether to render the global search input. Default: true */
    showGlobal?: boolean;
}


/**
 * Determines the default filter constraint configuration for a given column filter kind.
 * This function sets up the initial state of filters with appropriate match modes and empty values.
 *
 * @param kind - The type of filter to create default constraints for
 * @returns An object containing the default value (null) and appropriate match mode for the filter type
 */
function defaultConstraint(kind: ColumnFilterKind) {
    switch (kind) {
        case 'id':
            return { value: null, matchMode: FilterMatchMode.EQUALS };
        case 'text':
        case 'dropdown':
            return { value: null, matchMode: FilterMatchMode.CONTAINS };
        case 'number':
            // DataTable uses a single value for BETWEEN with array [min, max] or a small object – we’ll keep null start
            return { value: null, matchMode: FilterMatchMode.BETWEEN };
        case 'date':
            return [
                { value: null, matchMode: FilterMatchMode.DATE_AFTER },  // start
                { value: null, matchMode: FilterMatchMode.DATE_BEFORE }  // end
            ];
        case 'multiselect':
            return { value: null, matchMode: FilterMatchMode.IN };
        case 'boolean':
            return { value: null, matchMode: FilterMatchMode.EQUALS };
    }
}


/**
 * Determines the default filter operator for a given column filter kind.
 * This function defines how multiple filter constraints are combined for each 
 * filter type (AND/OR).
 *
 * @param kind - The type of filter to determine the operator for
 * @returns The appropriate FilterOperator (AND/OR) for the filter kind
 */
function defaultOperator(kind: ColumnFilterKind): FilterOperator {
    // UX-wise this often feels better for single-choice
    switch (kind) {
        case 'dropdown':
        case 'boolean':
            return FilterOperator.OR; 
    }

    return FilterOperator.AND;
}


/**
 * Builds the initial filter configuration for a DataTable based on the provided specification.
 * This function creates a filter metadata object that defines how each column should be filtered,
 * including the default values, match modes, and operators.
 *
 * @param spec - An object defining the filter types and behaviors for each field in the table
 * @returns A DataTableFilterMeta object containing the complete filter configuration
 */
export function buildDefaultFilters(spec: ModelSpec): DataTableFilterMeta {
    const meta: DataTableFilterMeta = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    };

    for (const [field, def] of Object.entries(spec)) {
        const operator = def.operator ?? defaultOperator(def.kind);
        const base = defaultConstraint(def.kind);
        const constraint = Array.isArray(base) ? 
            base : 
            [{
                value: base.value,
                matchMode: def.matchMode ?? base.matchMode,
            }];
        
        if (
            def.kind === 'id' ||
            def.kind === 'text' ||
            def.kind === 'date'
            // def.kind === 'dropdown'
        ) {
            meta[field] = {operator: operator, constraints: constraint};
        } else if (def.kind === 'number') {
            meta[field] = { value: null, matchMode: FilterMatchMode.BETWEEN };
        } else if (def.kind === 'multiselect' || def.kind === 'boolean') {
            meta[field] = constraint[0];
        }
    }

    return meta;
}

