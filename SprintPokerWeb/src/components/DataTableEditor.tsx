import { InputText } from 'primereact/inputtext';
import { InputNumber } from 'primereact/inputnumber';
import { Calendar } from 'primereact/calendar';
import { Dropdown } from 'primereact/dropdown';
import { MultiSelect } from 'primereact/multiselect';
import { Checkbox } from 'primereact/checkbox';
import { type ColumnSpec } from '../utils/DataTableColumn';
import type {ColumnEditorOptions} from "primereact/column";
import type { JSX } from "react";

interface DataTableEditorProps {
    field: string;
    columnSpec: ColumnSpec;
}

function DataTableEditor({
    field,
    columnSpec,
 }: DataTableEditorProps): ((options:ColumnEditorOptions) => JSX.Element) | null {
    switch (columnSpec.kind) {
        case 'text':
            return (options:ColumnEditorOptions) => (
                <InputText
                    value={options.value}
                    // NOTE: This is not clearly documented on PrimeReact's site, but is required to make the 
                    // editors work in the normal way without lag. Their examples show updating the entire table
                    // state in onChange, which is crazy. But the ColumnEditorOptions has a built-in 
                    // editorCallback that needs to be used so the values get stored in an internal cache which
                    // is then returned in the table's onRowEditComplete event so you can update the table's data.
                    // This same pattern is used in all the different editors below.
                    onChange={(e) => options.editorCallback?.(e.target.value)}
                    className="w-full"
                />
            );
        case "id":
        case 'number':
            return (options:ColumnEditorOptions) => (
                <InputNumber
                    value={options.value}
                    onChange={(e) => options.editorCallback?.(e.value)}
                    mode="decimal"
                    minFractionDigits={columnSpec.decimalPlaces ?? 0}
                    maxFractionDigits={columnSpec.decimalPlaces ?? 0}
                    readOnly={columnSpec.isReadOnly}
                />
            );
        case 'date':
            return (options:ColumnEditorOptions) => (
                <Calendar
                    value={options.value}
                    onChange={(e) => options.editorCallback?.(e.value)}
                    mask="9999-99-99 99:99"
                    dateFormat="yy-mm-dd"
                    showIcon
                    showTime
                    hourFormat="24"
                    readOnlyInput={columnSpec.isReadOnly}
                />
            );
        case 'dropdown':
            return (options:ColumnEditorOptions) => (
                <Dropdown
                    value={options.value}
                    options={columnSpec.selectItems}
                    onChange={(e) => options.editorCallback?.(e.value)}
                    readOnly={columnSpec.isReadOnly}
                />
            );
        case 'multiselect':
            return (options:ColumnEditorOptions) => (
                <MultiSelect
                    value={options.value}
                    options={columnSpec.selectItems}
                    onChange={(e) => options.editorCallback?.(e.value)}
                    readOnly={columnSpec.isReadOnly}
                />
            );
        case 'boolean':
            return (options:ColumnEditorOptions) => (
                <Checkbox
                    checked={options.value}
                    onChange={(e) => options.editorCallback?.(e.value)}
                    readOnly={columnSpec.isReadOnly}
                />
            );
        default:
            console.error(`DataTableEditor for field ${field} is not configured for ColumnSpec kind: ${columnSpec.kind}`);
            return null;
    }
}

export default DataTableEditor;