import { Column, type ColumnFilterElementTemplateOptions } from "primereact/column";
import {type ColumnSpec, getModelSelectItems} from '../utils/DataTableColumn';
import { type FilterCallback } from "../utils/DataTableFilterCache";
import {type CSSProperties} from "react";
import {formatDateCustom, formatNumber, formatHeaderText, type RowData} from "../utils/DataTableCellFormat";
import {FilterText} from "./FilterElement/FilterText";
import {FilterId} from "./FilterElement/FilterId";
import {FilterNumber} from "./FilterElement/FilterNumber";
import {FilterDate} from "./FilterElement/FilterDate";
import {FilterBoolean} from "./FilterElement/FilterBoolean";
import {FilterMultiselect} from "./FilterElement/FilterMultiselect"
import DataTableEditor from "./DataTableEditor";
import {Dropdown} from "primereact/dropdown";

const defaultWidth = '14rem';

/**
 * Props interface for the SwapiColumn component
 * @interface SmartColumnProps
 * @property {string} field - The field name for the column
 * @property {ColumnSpec} spec - Column specification containing display and behavior settings
 * @property {FilterCallback} filterCallbacks - Callbacks for filter operations
 * @property {number} [windowWidth] - Optional window width for responsive behavior
 */
interface SmartColumnProps {
    field: string;
    spec: ColumnSpec;
    filterCallbacks: FilterCallback;
    windowWidth?: number;
}

function SmartColumn({
    field,
    spec,
    filterCallbacks,
    windowWidth = 0,
}: SmartColumnProps) {
    const style: CSSProperties = {
        minWidth: spec.width ?? defaultWidth
    };
    
    /**
     * Returns the appropriate filter element component based on the column specification type
     * @returns {(opts: ColumnFilterElementTemplateOptions) => JSX.Element} Filter element component
     */
    const getFilterElement = () => {
        switch (spec.kind) {
            case 'id':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterId field={field} options={opts} filterCallbacks={filterCallbacks}/>;
            case 'number':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterNumber field={field} options={opts} filterCallbacks={filterCallbacks}/>;
            case 'date':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterDate field={field} options={opts} filterCallbacks={filterCallbacks}/>;
            case 'boolean':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterBoolean field={field} options={opts} filterCallbacks={filterCallbacks}/>;
            case 'multiselect':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterMultiselect
                        field={field} options={opts} filterCallbacks={filterCallbacks}
                        items={spec.selectItems ?? []}/>;
            default:
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterText field={field} options={opts} filterCallbacks={filterCallbacks}/>;
        }
    };

    /**
     * Returns the cell content formatter function based on the column specification type
     * @returns {(rowData: RowData) => string | number} Cell content formatter function
     */
    const getBody = () => {
        switch (spec.kind) {
            case 'number':
                return (rowData: RowData) => formatNumber(rowData, field, spec.decimalPlaces, spec.displaySuffix);
            case 'date':
                return (rowData: RowData) => formatDateCustom(rowData, field);
            case 'boolean':
                return (rowData: RowData) => rowData[field] ? 'Y' : 'N';
            case 'dropdown':
                return (rowData: RowData) => 
                    <Dropdown
                        value={rowData[field][0][spec.selectValueField ?? '']}
                        options={getModelSelectItems(spec, rowData[field])}
                        readOnly={true}
                    />;
            case 'multiselect':
                return "";
            default:
                return (rowData: RowData) => rowData[field] ?? '';
        }
    }

    /**
     * Determines the data type for the column based on specification
     * @returns {string} Column data type
     */
    const getDataType = () => {
        if (spec.dataType) return spec.dataType;
        if (spec.kind === 'number' || spec.kind === 'id') return 'numeric';
        return spec.kind;
    }

    return <Column
        field={field}
        hidden={spec.isHidden}
        dataType={getDataType()}
        header={formatHeaderText(field)}
        style={style}
        frozen={windowWidth > 768 ? spec.frozen : false}
        sortable
        filter
        showFilterMatchModes={spec.kind !== 'number'}
        filterElement={getFilterElement()}
        body={getBody()}
        editor={!spec.isReadOnly ? DataTableEditor({
            field: field,
            columnSpec: spec,
        }) : null}
        onFilterApplyClick={() => {
            filterCallbacks.applyCallbacks(field);
        }}
        onFilterClear={() => {
            filterCallbacks.clearCallbacks(field);
        }}
        
    />;
}

export default SmartColumn;