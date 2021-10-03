import { useMsal } from '@azure/msal-react';
import { useEffect, useState } from 'react';
import { loginRequestConfig } from '../../config';
import { getProfile, IProfile } from '../../services/GraphService';
import { ProfileView } from '../../features/profile/ProfileView';
import { Spinner } from '../../lib';

export const ProfilePage = () => {
    const { instance, accounts } = useMsal();
    const [profile, setProfile] = useState<IProfile>();
    const [loading, setLoading] = useState<boolean>();

    useEffect(() => {
        const loadProfileData = async () => {
            setLoading(true);
            try {
                const authResult = await instance.acquireTokenSilent({
                    ...loginRequestConfig,
                    account: accounts[0],
                });
                const profile = await getProfile(authResult.accessToken);
                setProfile(profile);
            } catch (error) {
                console.log(error);
            } finally {
                setLoading(false);
            }
        };
        loadProfileData();
    }, [accounts, instance]);

    return (
        <div>
            {loading && <Spinner />}
            {!loading && profile && (
                <>
                    <h1 className='mb-5' style={{textDecoration: 'underline', textUnderlineOffset: 12}}>PROFILE</h1>
                    <ProfileView profile={profile} />
                </>
            )}
        </div>
    );
};
