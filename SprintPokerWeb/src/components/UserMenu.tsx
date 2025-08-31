import { Menu } from 'primereact/menu';

function UserMenu() {
    const keycloakUrl = import.meta.env.VITE_KEYCLOAK_URL;
    const redirectUrl = encodeURIComponent(window.location.origin);
    
    let items = [
        { 
            label: 'My Account', 
            icon: 'pi pi-user', 
            url: `${keycloakUrl}/realms/sprintpoker/account/` 
        },
        { 
            label: 'Logout', 
            icon: 'pi pi-sign-out',
            url: `${keycloakUrl}/realms/sprintpoker/protocol/openid-connect/logout?redirect_uri=${redirectUrl}`
        }
    ];

    return (
        <Menu model={items} className="ml-auto"/>
    )
}

export default UserMenu;