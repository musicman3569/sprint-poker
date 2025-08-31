import Keycloak from 'keycloak-js';

export const keycloakConfig = {
    url: import.meta.env.VITE_KEYCLOAK_URL as string,
    realm: "sprintpoker",
    clientId: "sprintpoker",
};

const keycloak: Keycloak = new Keycloak(keycloakConfig);

// Guard against double init (e.g., React 18 StrictMode dev re-mount)
const originalInit = keycloak.init.bind(keycloak);
let initCalled = false;
(keycloak as any).init = (options?: Parameters<typeof originalInit>[0]) => {
    if (initCalled) {
        // Return resolved Promise to mimic successful init on subsequent calls
        return Promise.resolve(true);
    }
    initCalled = true;
    return originalInit(options);
};


export default keycloak;