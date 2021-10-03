import React from 'react';
import { IProfile } from '../../services';

interface IProfileViewProps {
    profile: IProfile;
}

export const ProfileView: React.FC<IProfileViewProps> = ({ profile }) => {
    return (
        <div>
            <div className='row mb-3 fs-4'>
                <label className='col-sm-2 col-form-label fw-bold'>
                    <i className='bi bi-card-heading me-3'></i>Display Name:
                </label>
                <label className='col-sm-10 col-form-label'>
                    {profile.displayName}
                </label>
            </div>
            <div className='row mb-3 fs-4'>
                <label className='col-sm-2 col-form-label fw-bold'>
                    <i className='bi bi-card-text me-3'></i>Full Name:
                </label>
                <label className='col-sm-10 col-form-label'>
                    {`${profile.firstName} ${profile.lastName}`}
                </label>
            </div>
            <div className='row mb-3 fs-4'>
                <label className='col-sm-2 col-form-label fw-bold'>
                    <i className='bi bi-envelope me-3'></i>Email:
                </label>
                <label className='col-sm-10 col-form-label'>
                    {profile.email}
                </label>
            </div>
            <div className='row mb-3 fs-4'>
                <label className='col-sm-2 col-form-label fw-bold'>
                    <i className='bi bi-person-badge me-3'></i>Job title:
                </label>
                <label className='col-sm-10 col-form-label'>
                    {profile.jobTitle}
                </label>
            </div>
        </div>
    );
};
