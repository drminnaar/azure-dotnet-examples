import { useMsal } from '@azure/msal-react';
import { PopupRequest } from '@azure/msal-browser';
import './LandingPage.styles.css';
import { loginRequestConfig } from '../../config';

export const LandingPage = () => {
    const { instance } = useMsal();

    const loginRequest: PopupRequest = {
        scopes: loginRequestConfig.scopes,
    };

    const signin = async () => {
        try {
            const result = await instance.loginRedirect(loginRequest);
            console.log(JSON.stringify(result));
        } catch (error) {
            console.log(error);
        }
    };

    return (
        <div className='landing-page'>
            <div className='signin'>
                <div className='display-1'>
                    <i className="bi bi-cloud-sun"></i>
                    <h1 className='display-1'>WONDERLAND WEATHER</h1>
                </div>
                <button
                    type='button'
                    className='btn btn-outline-light btn-lg'
                    onClick={() => signin()}
                >
                    <h1 className='display-6'>SIGNIN</h1>
                </button>
            </div>
        </div>
    );
};
