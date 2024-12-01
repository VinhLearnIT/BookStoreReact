import axios from 'axios';

const API_URL = 'https://localhost:7138/api/Customer';

const GetCustomers = async (data) => {
    try {
        const response = await axios.get(`${API_URL}/GetCustomers`);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            requestMsg: error.message
        };
    }
};

const GetCustomerById = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/GetCustomerById/${id}`);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            requestMsg: error.message
        };
    }
};

const CreateCustomer = async (token, data) => {
    try {
        const response = await axios.post(`${API_URL}/CreateCustomer`, data, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            requestMsg: error.message
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
            requestMsg: error.message
        };
    }
};

const DeleteCustomer = async (token, id) => {
    try {
        const response = await axios.delete(`${API_URL}/DeleteCustomer/${id}`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            requestMsg: error.message
        };
    }
};


export {
    GetCustomers,
    GetCustomerById,
    CreateCustomer,
    UpdateCustomer,
    DeleteCustomer
};
