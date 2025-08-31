import { useEffect, useState } from "react";
import type { FilterCallback } from "../../utils/DataTableFilterCache";
import type { ColumnFilterElementTemplateOptions } from "primereact/column";
import { Dropdown, type DropdownChangeEvent } from "primereact/dropdown";
import type { SelectItem } from "primereact/selectitem";

export function FilterDropdown({
    field,
    options,
    filterCallbacks,
    placeholder = 'Select...',
    items
}:{
    field: string;
    options: ColumnFilterElementTemplateOptions;
    filterCallbacks: FilterCallback;
    placeholder?: string;
    items: SelectItem[];
}) {
    // Maintain the filter's state internally to avoid bug with
    // default behavior refreshing table after each character
    const [draft, setDraft] = useState<any>(options.value ?? null);

    // if the table filter value changes externally, sync the editor
    useEffect(() => setDraft(options.value ?? null), [options.value]);

    return (
        <Dropdown
            value={draft}
            options={items}
            onChange={(e: DropdownChangeEvent) => {
                setDraft(e.value);
                filterCallbacks.setCallback(field, e.value, options);
            }}
            placeholder={placeholder}
            className="p-column-filter"
            showClear
        />
    );
}
