import React from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import LoginForm from '../Login/LoginForm';
import RegisterForm from '../Login/RegisterForm';
import ForgotPasswordForm from '../Login/ForgotPasswordForm';

function LoginPage() {
    return (
        <Routes>
            <Route path="/auth/" element={<Navigate to="/login" />} />
            <Route path="/login" element={<LoginForm />} />
            <Route path="/register" element={<RegisterForm />} />
            <Route path="/forgot-password" element={<ForgotPasswordForm />} />
        </Routes>
    );
}

export default LoginPage;
