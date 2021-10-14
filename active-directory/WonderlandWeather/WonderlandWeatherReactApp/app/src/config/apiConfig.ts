export const apiConfig = {
    scopes: process.env.REACT_APP_FORECAST_API_SCOPES?.split(',') ?? [],
    apiUrl: process.env.REACT_APP_FORECAST_API_URL ?? '',
    enabled: process.env.REACT_APP_FORECAST_API_ENABLED === 'true'
};
