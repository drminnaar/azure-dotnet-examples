import React from 'react';
import './Spinner.styles.css';

interface ISpinnerProps {
    content?: string;
    contentVisuallyHidden?: boolean;
}

export const Spinner: React.FC<ISpinnerProps> = ({
    content = 'Loading ...',
    contentVisuallyHidden = true,
}) => {

    const contentClass = contentVisuallyHidden ? 'visually-hidden' : '';

    return (
        <div className='spinner'>
            <div className='spinner-border text-primary'>
                <span className={contentClass}>{content}</span>
            </div>
        </div>
    );
};
