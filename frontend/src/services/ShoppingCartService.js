import axios from 'axios';

const API_URL = 'https://localhost:7138/api/ShoppingCart';



const GetCartByCustomerId = async (token, id) => {
    try {
        const response = await axios.get(`${API_URL}/GetCartByCustomerId/${id}`, {
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
const GetCountCartByCustomerId = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/GetCountCartByCustomerId/${id}`);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};
const CheckStockQuantity = async (bookId, quantity) => {
    try {
        const response = await axios.get(`${API_URL}/CheckStockQuantity?bookID=${bookId}&quantity=${quantity}`);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};


const AddToCart = async (token, data) => {
    try {
        const response = await axios.post(`${API_URL}/AddToCart`, data, {
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

const UpdateCart = async (token, id, data) => {
    try {
        const response = await axios.put(`${API_URL}/UpdateCart/${id}`, data, {
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

const DeleteCart = async (token, id) => {
    try {
        const response = await axios.delete(`${API_URL}/DeleteCart/${id}`, {
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
    GetCartByCustomerId,
    CheckStockQuantity,
    GetCountCartByCustomerId,
    AddToCart,
    UpdateCart,
    DeleteCart
};
