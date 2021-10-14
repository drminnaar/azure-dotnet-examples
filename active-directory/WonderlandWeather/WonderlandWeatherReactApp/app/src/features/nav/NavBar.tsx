import { useMsal } from '@azure/msal-react';
import { EndSessionRequest } from '@azure/msal-browser';
import { Link } from 'react-router-dom';

export const NavBar = () => {
    const { instance, accounts } = useMsal();

    const signoutRequest: EndSessionRequest = {
        postLogoutRedirectUri: '/',
    };

    const signout = async () => {
        try {
            await instance.logoutRedirect(signoutRequest);
        } catch (error) {
            console.log(error);
        }
    };

    return (
        <nav className='navbar navbar-light bg-primary'>
            <div className='container-fluid'>
                <a className='navbar-brand text-light' href='/'>
                    <h1>
                        <i className='bi bi-cloud-sun mx-3'></i>WONDERLAND WEATHER
                    </h1>
                </a>
                <div className='d-flex text-light'>
                    <div className='dropdown me-3'>
                        <button
                            className='btn btn-outline-light dropdown-toggle'
                            type='button'
                            data-bs-toggle='dropdown'
                            style={{ lineHeight: '30px' }}
                        >
                            <span className='align-middle'>
                                {accounts[0].username}
                            </span>
                            <i className='bi bi-person-circle h2 align-middle mx-2'></i>
                        </button>
                        <ul className='dropdown-menu'>
                            <li>
                                <Link
                                    to='/profile'
                                    className='dropdown-item'
                                    type='button'
                                >
                                    <i className='bi bi-info-circle pe-3'></i>
                                    Profile
                                </Link>
                            </li>
                            <li>
                                <Link
                                    to='/forecasts'
                                    className='dropdown-item'
                                    type='button'
                                >
                                    <i className='bi bi-info-circle pe-3'></i>
                                    Forecasts
                                </Link>
                            </li>
                            <li>
                                <button
                                    className='dropdown-item'
                                    type='button'
                                    onClick={() => signout()}
                                >
                                    <i className='bi bi-box-arrow-right pe-3'></i>
                                    Signout
                                </button>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </nav>
    );
};
