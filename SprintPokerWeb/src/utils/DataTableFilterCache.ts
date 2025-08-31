import { useCallback, useRef } from "react";
import type {ColumnFilterElementTemplateOptions} from "primereact/column";

export interface FilterCallbackCached {
    value: any;
    options: ColumnFilterElementTemplateOptions;
}

export interface FilterCallback {
    setCallback: (field:string, val: any, opts: ColumnFilterElementTemplateOptions) => void;
    applyCallbacks: (field: string) => void;
    clearCallbacks: (field: string) => void;
}

export function useCachedFilterCallbacks(): FilterCallback {
    const callbacks = useRef(new Map<string, Map<number, FilterCallbackCached>>());

    const setCallback = useCallback((field: string, val: any, opts: ColumnFilterElementTemplateOptions) => {
        const filterCallbackValue:FilterCallbackCached = {value: val, options: opts};
        
        if (!callbacks.current.has(field)) {
            callbacks.current.set(field, new Map<number, FilterCallbackCached>());
        }

        callbacks.current.get(field)!.set(opts.index, filterCallbackValue);
    }, []);

    const applyCallbacks = useCallback((field: string) => {
        const callbacksMap = callbacks.current.get(field);
        if (!callbacksMap) return;
        callbacksMap.forEach((callbackCache, index) => {
            callbackCache.options.filterApplyCallback(callbackCache.value, index);
        });
    }, []);

    const clearCallbacks = useCallback((field: string) => {
        callbacks.current.delete(field)
    }, []);

    return { setCallback, applyCallbacks, clearCallbacks };
}
