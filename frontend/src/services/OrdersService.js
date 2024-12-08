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
            message: error.response.data?.message
        };
    }
};

const GetOrderById = async (token, id) => {
    try {
        const response = await axios.get(`${API_URL}/GetOrderById/${id}`, {
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

const GetOrderByCustomerId = async (token, id) => {
    try {
        const response = await axios.get(`${API_URL}/GetOrderByCustomerId/${id}`,
            {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }
        );
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const AddOrderForCustomer = async (token, data) => {
    try {
        const response = await axios.post(`${API_URL}/AddOrderForCustomer`, data, {
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


const AddOrderForGuest = async (data) => {
    try {
        const response = await axios.post(`${API_URL}/AddOrderForGuest`, data);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
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
            message: error.response.data?.message
        };
    }
};
export {
    GetOrders,
    GetOrderByCustomerId,
    GetOrderById,
    AddOrderForCustomer,
    AddOrderForGuest,
    UpdateOrderStatus
};
