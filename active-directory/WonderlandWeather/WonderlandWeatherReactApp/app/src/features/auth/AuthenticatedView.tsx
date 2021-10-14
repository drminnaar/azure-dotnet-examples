import { BrowserRouter, Route } from 'react-router-dom';
import { NavBar } from '../nav/NavBar';
import { HomePage } from '../../pages/home/HomePage';
import { ProfilePage } from '../../pages/profile/ProfilePage';

export const AuthenticatedView = () => {
    return (
        <BrowserRouter>
            <NavBar />
            <div className='container-fluid px-3 pt-3'>
                <Route exact path='/' component={HomePage} />
                <Route exact path='/profile' component={ProfilePage} />
                <Route exact path='/forecasts' component={HomePage} />
            </div>
        </BrowserRouter>
    );
};
