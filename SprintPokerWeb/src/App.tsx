import { PrimeReactProvider } from 'primereact/api';
import 'primereact/resources/themes/soho-dark/theme.css'
import 'primeicons/primeicons.css'
import 'primeflex/primeflex.css'
import './App.css'
import { ReactKeycloakProvider } from "@react-keycloak/web";
import keycloak from "./utils/KeycloakInit"


function App() {
  return (
      <ReactKeycloakProvider
          authClient={keycloak}
          initOptions={{
              onLoad: 'login-required',
              pkceMethod: 'S256',
              checkLoginIframe: false,
          }}
      >
          <PrimeReactProvider>
              <div className="App">
                  <h1>Sprint Poker</h1>
              </div>
          </PrimeReactProvider>
      </ReactKeycloakProvider>
  );
}

export default App
