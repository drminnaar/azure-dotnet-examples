import {
    AuthenticatedTemplate,
    UnauthenticatedTemplate,
} from '@azure/msal-react';

// styles
import './App.css';

// components
import { AuthenticatedView } from './features/auth/AuthenticatedView';
import { UnauthenticatedView } from './features/auth/UnauthenticatedView';

function App() {
    return (
        <>
            <AuthenticatedTemplate>
                <AuthenticatedView />
            </AuthenticatedTemplate>

            <UnauthenticatedTemplate>
                <UnauthenticatedView />
            </UnauthenticatedTemplate>
        </>
    );
}

export { App };
