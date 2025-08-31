import { type ModelSpec } from '../utils/DataTableColumn';

const ModelCardSet: ModelSpec = {
    CardSetId: {kind: 'id', width: '12rem', isDataKey: true, isHidden: false, isReadOnly: true},
    Name: {kind: 'text'},
    CreateAt: {kind: 'date', isReadOnly: true},
    CreatedBy: {kind: 'text', isReadOnly: true},
    ModifiedAt: {kind: 'date', isReadOnly: true},
    ModifiedBy: {kind: 'text', isReadOnly: true},
};

export default ModelCardSet;