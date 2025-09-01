import { type ModelSpec } from '../utils/DataTableColumn';

const ModelCard: ModelSpec = {
    CardId: {kind: 'id', width: '12rem', isDataKey: true, isHidden: false, isReadOnly: true},
    Value: {kind: 'number'},
    DisplayName: {kind: 'text'},
    ModifiedAt: {kind: 'date', isReadOnly: true},
    ModifiedBy: {kind: 'text', isReadOnly: true},
    CreatedAt: {kind: 'date', isReadOnly: true},
    CreatedBy: {kind: 'text', isReadOnly: true},
};

export default ModelCard;