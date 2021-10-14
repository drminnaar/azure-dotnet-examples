import React from 'react';
import moment from 'moment';

// styles
import './ForecastsTable.styles.css';

// services
import { IForecast } from '../../../services';

interface IForecastsTableProps {
    forecasts: IForecast[];
}

export const ForecastsTable: React.FC<IForecastsTableProps> = ({
    forecasts,
}) => {
    return (
        <div className='table-responsive'>
            <table className='table table-striped table-hover'>
                <thead>
                    <tr>
                        <th scope='col'>Date</th>
                        <th scope='col'>Temp C</th>
                        <th scope='col'>Temp F</th>
                        <th scope='col'>Summary</th>
                    </tr>
                </thead>
                {!forecasts && (
                    <div className='alert alert-secondary' role='alert'>
                        No forecast data available
                    </div>
                )}
                {forecasts && (
                    <tbody>
                        {forecasts.map(forecast => (
                            <tr key={forecast.id} className='align-middle'>
                                <td>{moment(forecast.date).format('DD MMM yyyy HH:mm:ss')}</td>
                                <td>{forecast.temperatureC}</td>
                                <td>{forecast.temperatureF}</td>
                                <td>{forecast.summary}</td>
                            </tr>
                        ))}
                    </tbody>
                )}
            </table>
        </div>
    );
};
