import { PrimeReactProvider } from 'primereact/api';
import './assets/theme.css'
import 'primeicons/primeicons.css'
import 'primeflex/primeflex.css'
import './App.css'
import { ReactKeycloakProvider } from "@react-keycloak/web";
import {TabPanel, TabView} from "primereact/tabview";
import keycloak from "./utils/KeycloakInit"
import {useState} from "react";
import UserMenu from "./components/UserMenu.tsx";


function App() {
    const [activeTabIndex, setActiveTabIndex] = useState(0);
    
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
              <TabView activeIndex={activeTabIndex} onTabChange={(e) => setActiveTabIndex(e.index)}>
                  <TabPanel className="border-round-top-xl mr-2" header={<>
                      <span>Current Game</span>
                  </>}>
                      <h1>Game</h1>
                  </TabPanel>
                  <TabPanel className="border-round-top-xl mr-2" header={<>
                      <i className="pi pi-clone"></i> <span>Card Sets</span>
                  </>}>
                      <h1>Card Sets</h1>
                  </TabPanel>
                  <TabPanel className="border-round-top-xl mr-2" headerClassName="tab-rightmost" header={<>
                      <i className="pi pi-bars mr-2"></i>
                  </>}>
                      <div className="flex">
                          <UserMenu />
                      </div>
                  </TabPanel>
              </TabView>
          </PrimeReactProvider>
      </ReactKeycloakProvider>
  );
}

export default App
