import React from 'react';
import { IForecast } from '../../../services';
import { ForecastsTable } from './ForecastsTable';

interface IForecastsDashboardProps {
    forecasts: IForecast[];
}

export const ForecastsDashboard: React.FC<IForecastsDashboardProps> = ({
    forecasts,
}) => {
    return <ForecastsTable forecasts={forecasts} />;
};
