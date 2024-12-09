import axios from 'axios';

const API_URL = 'https://localhost:7138/api/Category';

const GetCategories = async (data) => {
    try {
        const response = await axios.get(`${API_URL}/GetCategories`);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};



const CreateCategory = async (token, data) => {
    try {
        const response = await axios.post(`${API_URL}/CreateCategory`, data, {
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

const UpdateCategory = async (token, id, data) => {
    try {
        const response = await axios.put(`${API_URL}/UpdateCategory/${id}`, data, {
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

const DeleteCategory = async (token, id) => {
    try {
        const response = await axios.delete(`${API_URL}/DeleteCategory/${id}`, {
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
    GetCategories,
    CreateCategory,
    UpdateCategory,
    DeleteCategory
};
