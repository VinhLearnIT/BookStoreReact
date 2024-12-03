import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

const PrivateRoute = ({ allowedRoles }) => {
    const isLogin = localStorage.getItem('isAuthenticated');
    const userRole = localStorage.getItem('role');
    const expireTime = localStorage.getItem('expireTime');
    const currentTime = new Date();

    if (isLogin === 'true' && allowedRoles.includes(userRole) && new Date(expireTime) > currentTime) {
        return <Outlet />;
    }

    return <Navigate to="/auth/login" replace />;
};

export default PrivateRoute;
