import axios from 'axios';

const API_URL = 'https://localhost:7138/api/Order';

const GetOrders = async (token) => {
    try {
        const response = await axios.get(`${API_URL}/GetOrders`, {
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

const GetOrderById = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/GetOrderById/${id}`);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            requestMsg: error.message
        };
    }
};

const CreateOrder = async (token, data) => {
    try {
        const response = await axios.post(`${API_URL}/CreateOrder`, data, {
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


const UpdateOrder = async (token, id, data) => {
    try {
        const response = await axios.put(`${API_URL}/UpdateOrder/${id}`, data, {
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

const UpdateOrderStatus = async (token, id, data) => {
    try {
        const response = await axios.put(`${API_URL}/UpdateOrderStatus/${id}`, data, {
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
const DeleteOrder = async (token, id) => {
    try {
        const response = await axios.delete(`${API_URL}/DeleteOrder/${id}`, {
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
    GetOrders,
    GetOrderById,
    CreateOrder,
    UpdateOrder,
    UpdateOrderStatus,
    DeleteOrder
};
