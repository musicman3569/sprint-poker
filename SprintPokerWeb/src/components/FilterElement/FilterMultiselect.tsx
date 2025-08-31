import { useEffect, useState } from "react";
import type { FilterCallback } from "../../utils/DataTableFilterCache";
import type { ColumnFilterElementTemplateOptions } from "primereact/column";
import type { SelectItem } from "primereact/selectitem";
import { MultiSelect, type MultiSelectChangeEvent } from "primereact/multiselect";

export function FilterMultiselect({
    field,
    options,
    filterCallbacks,
    placeholder = 'Select...',
    items,
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
        <MultiSelect
            value={draft}
            options={items}
            onChange={(e: MultiSelectChangeEvent) => {
                setDraft(e.value);
                filterCallbacks.setCallback(field, e.value, options);
            }}
            placeholder={placeholder}
            className="p-column-filter"
            display="chip"
            showClear
        />
    );
}
