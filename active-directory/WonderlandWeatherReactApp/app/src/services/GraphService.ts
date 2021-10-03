import { graphConfig } from '../config';

export interface IProfile {
    firstName: string;
    lastName: string;
    email: string;
    id: string;
    displayName: string;
    jobTitle: string;
}

export async function getProfile(accessToken: string): Promise<IProfile> {
    const headers = new Headers();
    const bearer = `Bearer ${accessToken}`;

    headers.append('Authorization', bearer);

    const options = {
        method: 'GET',
        headers: headers,
    };

    try {
        const response = await fetch(graphConfig.graphMeEndpoint, options);
        const profileData = await response.json();
        return {
            firstName: profileData.givenName ?? '',
            lastName: profileData.surname ?? '',
            displayName: profileData.displayName ?? '',
            email: profileData.mail ?? 'NA',
            jobTitle: profileData.jobTitle ?? 'NA',
            id: profileData.userPrincipalName,
        };
    } catch (error) {
        console.log(error);
        throw new Error(`Get profile data failed`);
    }
}
