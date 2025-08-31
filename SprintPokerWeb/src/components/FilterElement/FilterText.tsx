import { useEffect, useState } from "react";
import { InputText } from "primereact/inputtext";
import type { FilterCallback } from "../../utils/DataTableFilterCache";
import type { ColumnFilterElementTemplateOptions } from "primereact/column";

export function FilterText({ 
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
    const [draft, setDraft] = useState<string>(options.value ?? "");

    // if the table filter value changes externally, sync the editor
    useEffect(() => setDraft(options.value ?? ""), [options.value]);

    return (
        <InputText
            value={draft}
            onChange={(e) => { 
                setDraft(e.target.value);
                filterCallbacks.setCallback(field, e.target.value, options); 
            }}
            placeholder="Type to filter…"
            className="p-column-filter"
        />
    );
}
