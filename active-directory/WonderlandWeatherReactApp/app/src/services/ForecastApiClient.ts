const data = require('./forecasts.json');

export const fetchForecasts = <T>(): Promise<T> => {
    try {
        return new Promise((resolve, reject) => {
            resolve(data);
        });
    } catch (error) {
        console.log(error);
        throw new Error(`Wonderland Weather API Error: ${error}`);
    }
};
