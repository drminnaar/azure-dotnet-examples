import { fetchForecasts } from './ForecastApiClient';

export interface IForecast {
    id: string;
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

export const getForecasts = async (accessToken: string) => await fetchForecasts<IForecast[]>(accessToken);
