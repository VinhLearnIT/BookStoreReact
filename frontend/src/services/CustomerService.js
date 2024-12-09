import axios from 'axios';

const API_URL = 'https://localhost:7138/api/Customer';

const GetCustomers = async (token) => {
    try {
        const response = await axios.get(`${API_URL}/GetCustomers`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const GetCustomerById = async (token, id) => {
    try {
        const response = await axios.get(`${API_URL}/GetCustomerById/${id}`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};


const UpdateCustomer = async (token, id, data) => {
    try {
        const response = await axios.put(`${API_URL}/UpdateCustomer/${id}`, data, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};
const UpdatePassword = async (token, data) => {
    try {
        const response = await axios.put(`${API_URL}/UpdatePassword`, data, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};
const UpdateCustomerRole = async (token, id, data) => {
    try {
        const response = await axios.put(`${API_URL}/UpdateCustomerRole/${id}`, data, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const UpdateCustomerStatus = async (token, id, data) => {
    try {
        const response = await axios.put(`${API_URL}/UpdateCustomerStatus/${id}`, data, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
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
    GetCustomers,
    GetCustomerById,
    UpdatePassword,
    UpdateCustomerRole,
    UpdateCustomer,
    UpdateCustomerStatus
};
