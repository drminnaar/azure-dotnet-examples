import ReactDOM from 'react-dom';
import { PublicClientApplication } from '@azure/msal-browser';

// config
import { msalConfig } from './config';

// styles
import './index.css';

// components
import { App } from './App';
import { MsalProvider } from '@azure/msal-react';

const msalInstance = new PublicClientApplication(msalConfig);

ReactDOM.render(
    <MsalProvider instance={msalInstance}>
        <App />
    </MsalProvider>,
    document.getElementById('root')
);
