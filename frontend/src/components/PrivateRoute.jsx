import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

const PrivateRoute = ({ allowedRoles }) => {
    const userRole = localStorage.getItem('role');
    const isAuthenticated = localStorage.getItem('isAuthenticated') === 'true';
    const expireTime = localStorage.getItem('expireTime');
    const currentTime = new Date();

    if (isAuthenticated && allowedRoles.includes(userRole) && expireTime && new Date(expireTime) > currentTime) {
        return <Outlet />;
    }

    return <Navigate to="/auth/login" replace />;
};

export default PrivateRoute;
