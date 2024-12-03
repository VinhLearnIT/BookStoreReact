import React from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import PrivateRoute from '../components/PrivateRoute';
import AdminLayout from '../layouts/AdminLayout'
import DashBoard from '../pages/admin/DashBoard';
import ManageBooks from '../pages/admin/ManageBooks';
import ManageCategory from '../pages/admin/ManageCategory';
import ManageOrder from '../pages/admin/ManageOrder';
import ManageCustomer from '../pages/admin/ManageCustomer';

const AdminRoutes = () => {
    return (

        <Routes>
            <Route element={<PrivateRoute allowedRoles={['Admin', 'Manager']} />}>
                <Route element={<AdminLayout />}>
                    <Route path="/" element={<Navigate to="/admin/dash-board" />} />
                    <Route path="/dash-board" element={<DashBoard />} />
                    <Route path="/manage-books" element={<ManageBooks />} />
                    <Route path="/manage-category" element={<ManageCategory />} />
                    <Route path="/manage-order" element={<ManageOrder />} />
                    <Route path="/manage-customer" element={<ManageCustomer />} />
                </Route>
            </Route>
        </Routes>
    )
}

export default AdminRoutes;