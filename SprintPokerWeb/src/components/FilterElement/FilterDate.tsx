import { useEffect, useState } from "react";
import { Calendar } from "primereact/calendar";
import type { FilterCallback } from "../../utils/DataTableFilterCache";
import type { ColumnFilterElementTemplateOptions } from "primereact/column";

export function FilterDate({
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
    const [draft, setDraft] = useState<Date | null>(options.value ?? null);

    // if the table filter value changes externally, sync the editor
    useEffect(() => setDraft(options.value ?? ""), [options.value]);

    return (
        <Calendar
            value={draft}
            onChange={(e) => {
                setDraft(e.value ?? null);
                filterCallbacks.setCallback(field, e.value, options);
            }}
            placeholder={options.index === 0 ? 'Start date' : 'End date'}
            mask="9999-99-99 99:99"
            dateFormat="yy-mm-dd"
            showIcon
            showTime
            hourFormat="24"
        />
    );
}
