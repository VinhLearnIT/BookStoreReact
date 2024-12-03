import React from 'react';
import { Outlet } from 'react-router-dom';

const UserLayout = () => {
    return (
        <div className="flex justify-center items-center h-screen bg-slate-200">
            <Outlet />
        </div>
    );
};

export default UserLayout;