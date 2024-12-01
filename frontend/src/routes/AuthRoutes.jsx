import React from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import AuthLayout from '../layouts/AuthLayout'
import LoginForm from '../pages/auth/LoginForm';
import RegisterForm from '../pages/auth/RegisterForm';
import ForgotPasswordForm from '../pages/auth/ForgotPasswordForm';

function AuthRouter() {
    return (
        <Routes>
            <Route element={<AuthLayout />}>
                <Route path="/" element={<Navigate to="/auth/login" />} />
                <Route path="/login" element={<LoginForm />} />
                <Route path="/register" element={<RegisterForm />} />
                <Route path="/forgot-password" element={<ForgotPasswordForm />} />
            </Route>
        </Routes>
    );
}

export default AuthRouter;
