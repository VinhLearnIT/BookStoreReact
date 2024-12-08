import axios from 'axios';

const API_URL = 'https://localhost:7138/api/Auth';

const Login = async (data) => {
    try {
        const response = await axios.post(`${API_URL}/Login`, data);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const Register = async (data) => {
    try {
        const response = await axios.post(`${API_URL}/Register`, data);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const SendVerificationCode = async (data) => {
    try {
        const response = await axios.post(`${API_URL}/SendVerificationCode`, data);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const SendMailContact = async (data) => {
    try {
        const response = await axios.post(`${API_URL}/SendMailContact`, data);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const ForgotPassword = async (data) => {
    try {
        const response = await axios.put(`${API_URL}/ForgotPassword`, data);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const RefreshToken = async (data) => {
    try {
        const response = await axios.post(`${API_URL}/RefreshToken`, data);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

export {
    Login,
    Register,
    SendVerificationCode,
    SendMailContact,
    ForgotPassword,
    RefreshToken
};
