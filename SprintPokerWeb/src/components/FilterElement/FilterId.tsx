import { useEffect, useState } from "react";
import { InputNumber } from "primereact/inputnumber";
import type { FilterCallback } from "../../utils/DataTableFilterCache";
import type { ColumnFilterElementTemplateOptions } from "primereact/column";
import type { InputNumberChangeEvent } from "primereact/inputnumber";

export function FilterId({
   field,
   options,
   filterCallbacks
}:{
    field: string;
    options: ColumnFilterElementTemplateOptions;
    filterCallbacks: FilterCallback;
}) {
    // Maintain the filter's state internally to avoid bug with
    // default behavior refreshing table after each character
    const [draft, setDraft] = useState<number | null>(options.value ?? null);

    // if the table filter value changes externally, sync the editor
    useEffect(() => setDraft(options.value), [options.value]);

    return (
        <InputNumber
            value={draft}
            onChange={(e: InputNumberChangeEvent) => {
                setDraft(e.value);
                filterCallbacks.setCallback(field, e.value, options);
            }}
            className="p-column-filter"
        />
    );
}
