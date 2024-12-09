import React from 'react';
import { Outlet } from 'react-router-dom';
import UserHeader from '../components/UserHeader';
import UserFooter from '../components/UserFooter';
const UserLayout = () => {
    return (
        <>
            <UserHeader />

            <div style={{ minHeight: 'calc(100vh - 15rem)' }}>
                <Outlet />
            </div>

            <UserFooter />
        </>
    );
};

export default UserLayout;