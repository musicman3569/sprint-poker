import {Dialog} from 'primereact/dialog';
import {Button} from 'primereact/button';
import {type JSX, useState} from 'react';
import {type ModelSpec} from '../utils/DataTableColumn';
import DataTableEditor from './DataTableEditor';
import type {ColumnEditorOptions} from "primereact/column";
import { formatHeaderText } from "../utils/DataTableCellFormat";

interface DataTableEditFormProps {
    visible: boolean;
    onHide: () => void;
    modelSpec: ModelSpec;
    data?: any;
    onSave: (data: any) => void;
}

interface EditorComponent {
    field: string;
    element?: JSX.Element;
}

const DataTableEditForm = ({
   visible,
   onHide,
   modelSpec,
   data = [] as any[],
   onSave
}: DataTableEditFormProps) => {
    const [formData, setFormData] = useState(data ?? []);

    const resetForm = () => {
        setFormData([]);
        onHide();
    }
    
    const footerContent = (
        <div>
            <Button label="Cancel" icon="pi pi-times" onClick={resetForm} className="p-button-text"/>
            <Button label="Save" icon="pi pi-check" onClick={() => {
                onSave(formData);
                resetForm();
            }} autoFocus/>
        </div>
    );
    

    const editorComponents = Object
        .entries(modelSpec)
        .filter(([, columnSpec]) => !columnSpec.isReadOnly)
        .map(([field, columnSpec]) => {
            return {
                field: field,
                element: DataTableEditor({field, columnSpec})?.({
                    value: formData[field] ?? '', // must always be defined to not get an uncontrolled component warning in the browser
                    field: field,
                    rowData: formData,
                    editorCallback: (value: any) => {
                        setFormData({...formData, [field]: value});
                    }
                } as ColumnEditorOptions)
            } as EditorComponent;
        })
        .filter((e)=> e.element !== undefined)
    ;

    return (
        <Dialog
            header="Edit Record"
            visible={visible}
            onShow={() => setFormData(data ?? [])}
            onHide={() => false}
            style={{width: '50vw', minWidth: '28rem'}}
            footer={footerContent}
            modal
        >
            <div className="p-fluid">
                {editorComponents.map( (editor:EditorComponent) =>(
                    <div className="p-inputgroup" key={editor.field}>
                        <span className="p-inputgroup-addon swapi-form-label">
                            {formatHeaderText(editor.field)}
                        </span>
                        {editor.element}
                    </div>
                ))}
            </div>
        </Dialog>
    );
};

export default DataTableEditForm;