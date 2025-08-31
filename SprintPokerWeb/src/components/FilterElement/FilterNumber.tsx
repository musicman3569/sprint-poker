import { useEffect, useState } from "react";
import { InputNumber } from "primereact/inputnumber";
import type { FilterCallback } from "../../utils/DataTableFilterCache";
import type { ColumnFilterElementTemplateOptions } from "primereact/column";
import type { InputNumberChangeEvent } from "primereact/inputnumber";

export function FilterNumber({
    field,
    options,
    filterCallbacks
}: {
    field: string;
    options: ColumnFilterElementTemplateOptions;
    filterCallbacks: FilterCallback;
}) {
    // Maintain the filter's state internally to avoid bug with
    // default behavior refreshing table after each character
    const [min, setMin] = useState<number | null>(options.value?.[0] ?? null);
    const [max, setMax] = useState<number | null>(options.value?.[1] ?? null);

    // if the table filter value changes externally, sync the editor
    useEffect(() => setMin(options.value?.[0] ?? null), [options.value?.[0]]);
    useEffect(() => setMax(options.value?.[1] ?? null), [options.value?.[1]]);

    return (
        <div className="flex flex-column gap-2 p-column-filter">
            <InputNumber
                value={min}
                onChange={(e: InputNumberChangeEvent) => {
                    setMin(e.value);
                    filterCallbacks.setCallback(field, [e.value, max || Infinity], options);
                }}
                placeholder="Min"
            />
            <InputNumber
                value={max}
                onChange={(e: InputNumberChangeEvent) => {
                    setMax(e.value);
                    filterCallbacks.setCallback(field, [min || -Infinity, e.value], options);
                }}
                placeholder="Max"
            />
        </div>
    );
}