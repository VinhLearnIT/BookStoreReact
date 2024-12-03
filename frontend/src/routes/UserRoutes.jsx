import React from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import PrivateRoute from '../components/PrivateRoute';
import UserLayout from '../layouts/UserLayout'
import Home from '../pages/user/Home';

const AdminRoutes = () => {
    return (

        <Routes>
            <Route element={<PrivateRoute allowedRoles={['User', 'Admin', 'Manager']} />}>
                <Route element={<UserLayout />}>
                    <Route path="/" element={<Navigate to="/user/home" />} />
                    <Route path="/home" element={<Home />} />
                </Route>
            </Route>
        </Routes>
    )
}

export default AdminRoutes;