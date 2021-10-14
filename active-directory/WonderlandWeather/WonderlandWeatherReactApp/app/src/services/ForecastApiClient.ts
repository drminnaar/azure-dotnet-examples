import { apiConfig } from '../config';

export const fetchForecasts = async <T>(
    accessToken: string = ''
): Promise<T> => {
    try {
        return apiConfig.enabled
            ? await fetchForecastsFromApi(accessToken)
            : fetchForecastsFromFile();
    } catch (error) {
        console.log(error);
        throw new Error(`Wonderland Weather API Error: ${error}`);
    }
};

const fetchForecastsFromFile = <T>(): Promise<T> => {
    return new Promise((resolve, reject) => {
        resolve(require('./forecasts.json'));
    });
};

const fetchForecastsFromApi = async <T>(accessToken: string): Promise<T> => {
    const options: RequestInit = {
        method: 'GET',
        headers: {
            Authorization: `Bearer ${accessToken}`,
        },
    };

    const response = await fetch(
        `${apiConfig.apiUrl}/forecasts`,
        options
    );

    return await response.json();
};
