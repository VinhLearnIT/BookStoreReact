import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

const PrivateRoute = () => {
    const isLogin = sessionStorage.getItem('isAuthenticated');
    return isLogin === 'true' ? <Outlet /> : <Navigate to="/auth/login" replace />;
};

export default PrivateRoute;
