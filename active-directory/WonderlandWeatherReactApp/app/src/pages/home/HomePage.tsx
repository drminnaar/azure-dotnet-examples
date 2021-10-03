import { useState } from 'react';
import { getForecasts, IForecast } from '../../services';
import { ForecastsDashboard } from '../../features/forecasts/dashboard/ForecastsDashboard';

export const HomePage = () => {
    const [forecasts, setForecasts] = useState<IForecast[]>([]);

    const loadForecasts = async () => {
        const forecastsFromService = await getForecasts();
        setForecasts(forecastsFromService ?? []);
    };

    return (
        <div>
            <h1>HOME</h1>
            <div className="my-3">
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
