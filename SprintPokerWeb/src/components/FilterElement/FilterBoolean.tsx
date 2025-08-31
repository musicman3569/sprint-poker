import type { FilterCallback } from "../../utils/DataTableFilterCache";
import type { ColumnFilterElementTemplateOptions } from "primereact/column";
import { FilterDropdown } from "./FilterDropdown.tsx";

export function FilterBoolean({
    field,
    options,
    filterCallbacks
}:{
    field: string;
    options: ColumnFilterElementTemplateOptions;
    filterCallbacks: FilterCallback;
}) {
    return FilterDropdown({
        field: field,
        options: options,
        filterCallbacks: filterCallbacks,
        placeholder: 'Any',
        items: [
            {label: 'Yes', value: true},
            {label: 'No', value: false}
        ]
    });
}
