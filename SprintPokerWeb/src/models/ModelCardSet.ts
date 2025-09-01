import { type ModelSpec } from '../utils/DataTableColumn';

const ModelCardSet: ModelSpec = {
    Name: {kind: 'text', isDisplayName: true, frozen: true},
    CardSetId: {kind: 'id', width: '12rem', isDataKey: true, isHidden: true, isReadOnly: true},
    Cards: {kind: "dropdown", selectValueField: "Value", selectLabelField: "DisplayName"},
    ModifiedAt: {kind: 'date', isReadOnly: true},
    ModifiedBy: {kind: 'text', isReadOnly: true},
    CreatedAt: {kind: 'date', isReadOnly: true},
    CreatedBy: {kind: 'text', isReadOnly: true},
};

export default ModelCardSet;