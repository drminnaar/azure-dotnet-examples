import { useState } from 'react';
import { getForecasts, IForecast } from '../../services';
import { ForecastsDashboard } from '../../features/forecasts/dashboard/ForecastsDashboard';
import { useMsal } from '@azure/msal-react';
import { apiConfig } from '../../config';

export const HomePage = () => {
    const { instance, accounts } = useMsal();
    const [forecasts, setForecasts] = useState<IForecast[]>([]);

    const tokenRequest = {
        scopes: apiConfig.scopes,
        forceRefresh: false, // Set this to "true" to skip a cached token and go to the server to get a new token
    };

    const loadForecasts = async () => {
        const authResult = await instance.acquireTokenSilent({
            ...tokenRequest,
            account: accounts[0],
        });

        const forecastsFromService = await getForecasts(authResult.accessToken);
        setForecasts(forecastsFromService ?? []);
    };

    return (
        <div>
            <h1>HOME</h1>
            <div className='my-3'>
                <button
                    className='btn btn-primary btn-lg'
                    onClick={() => loadForecasts()}
                >
                    LOAD FORECASTS
                </button>
            </div>
            <ForecastsDashboard forecasts={forecasts} />
        </div>
    );
};
