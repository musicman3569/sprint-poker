import { convertStringDates } from "./DateUtils";
import { type ModelSpec } from "./DataTableColumn";

/**
 * Fetches data from the Star Wars API and processes date fields.
 *
 * @param modelSpec - The model specification containing field definitions
 * @param modelDataKey - The key field name in the model (usually ends with '_id')
 * @param successCallback - React setState callback function to update the component's state with the fetched data
 * @param authToken - The OAuth Bearer access token for the API request
 * @example
 * // Fetch "starships" data and convert 'created' and 'edited' fields to Date objects
 * const [starships, setStarships] = useState<any[]>([]);
 * FetchData('starships', ['created', 'edited'], setStarships);
 *
 * @throws {Error} Logs an error message to console if the fetch operation fails
 */
export async function FetchData(
    modelSpec: ModelSpec,
    modelDataKey: string,
    successCallback: (data: any[]) => void,
    authToken?: string,
) {
    const apiUrl = getApiUrl(modelDataKey);

    try {
        const response = await fetch(apiUrl.fullUrl, {
            headers: {
                'Authorization': 'Bearer ' + authToken,
                'Content-Type': 'application/json'
            }
        });
        const data = await response.json();
        successCallback(
            convertStringDates(data, getDateFields(modelSpec))
        );
    } catch (error) {
        console.log('Error fetching ' + apiUrl.path + ' data: ', error);
    }
}


/**
 * Updates or creates a record in the Star Wars API.
 *
 * @param modelSpec - The model specification containing field definitions
 * @param modelDataKey - The key field name in the model (usually ends with '_id')
 * @param newData - The data to be updated or created. If the modelDataKey exists, performs PUT; otherwise, performs POST
 * @param successCallback - Callback function to handle the updated/created data
 * @param authToken - The OAuth Bearer access token for the API request
 * 
 * @example
 * // Update a starship record
 * UpdateData(starshipSpec, 'starship_id', updatedStarship, setSelectedStarship);
 *
 * @throws {Error} Logs an error message to console if the update operation fails
 */
export async function UpdateData(
    modelSpec: ModelSpec,
    modelDataKey: string,
    newData: any,
    successCallback: (responseData: any) => void,
    authToken?: string,
) {
    const apiUrl = getApiUrl(modelDataKey);

    try {
        const response = await fetch(apiUrl.fullUrl, {
            method: (newData[modelDataKey]) ? 'PUT' : 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + authToken
            },
            body: JSON.stringify(newData)
        });
        const updatedModel = await response.json();
        successCallback(
            convertStringDates([updatedModel], getDateFields(modelSpec)).at(0) as any
        );
    } catch (error) {
        console.log('Error updating ' + apiUrl.path + ' data: ', error);
    }
}

/**
 * Deletes a record from the Star Wars API.
 *
 * @param modelDataKey - The key field name in the model (usually ends with '_id')
 * @param id - The unique identifier of the record to delete
 * @param successCallback - Callback function to execute after successful deletion
 * @param authToken - The OAuth Bearer access token for the API request
 *
 * @example
 * // Delete a starship record
 * DeleteData('starship_id', '5', () => refreshData());
 *
 * @throws {Error} Throws an error if the deletion operation fails
 */
export async function DeleteData(
    modelDataKey: string,
    id: string,
    successCallback: () => void,
    authToken?: string,
) {
    const apiUrl = getApiUrl(modelDataKey);
    const url = apiUrl.fullUrl + '/' + id;
    try {
        const response = await fetch(url, {
            method: 'DELETE',
            headers: {
                'Authorization': 'Bearer ' + authToken
            }
        });
        if (response.ok) {
            successCallback();
        } else {
            throw new Error('Failed to delete ' + apiUrl.path + ' data');
        }
    } catch (error) {
        throw new Error('Failed to delete ' + apiUrl.path + ' data: ' + error);
    }
}

/**
 * Generates the API URL for a specific model.
 *
 * @param modelDataKey - The key field name in the model (usually ends with '_id')
 * @returns An object containing the full URL and the path component
 *
 * @example
 * // Get API URL for starships
 * const url = getApiUrl('starship_id'); // Returns { fullUrl: 'api/starship', path: 'starship' }
 */
function getApiUrl(modelDataKey: string) {
    const apiUrl = import.meta.env.VITE_API_URL;
    const apiPath = modelDataKey.replace('Id', '');
    return {
        fullUrl: apiUrl + '/' + apiPath,
        path: apiPath,
    };
}

/**
 * Extracts field names that represent dates from the model specification.
 *
 * @param modelSpec - The model specification containing field definitions
 * @returns An array of field names that are marked as date fields
 *
 * @example
 * // Get date fields from starship specification
 * const dateFields = getDateFields(starshipSpec); // Returns ['created', 'edited']
 */
function getDateFields(modelSpec: ModelSpec) {
    return Object.keys(modelSpec).filter(field =>
        modelSpec[field].dataType === 'date' ||
        modelSpec[field].kind === 'date'
    );
}