import axios from 'axios';

const API_URL = 'https://localhost:7138/api/Book';

const GetBooks = async (data) => {
    try {
        const response = await axios.get(`${API_URL}/GetBooks`);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const GetTopBooks = async (data) => {
    try {
        const response = await axios.get(`${API_URL}/GetTopBooks`);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const GetBookById = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/GetBookById/${id}`);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const SearchBooks = async (searchName) => {
    try {
        const response = await axios.get(`${API_URL}/SearchBooks/${searchName}`);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};


const GetRelatedBooks = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/GetRelatedBooks/${id}`);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const GetNewInfoBooks = async (arrId) => {
    try {
        const response = await axios.get(`${API_URL}/GetNewInfoBooks/${arrId}`);
        return response;
    } catch (error) {
        return {
            status: error.response.status,
            data: error.response.data,
            message: error.response.data?.message
        };
    }
};

const CreateBook = async (token, data) => {
    try {
        const response = await axios.post(`${API_URL}/CreateBook`, data, {
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

const UploadImage = async (token, data) => {
    try {
        const response = await axios.post(`${API_URL}/UploadImage`, data, {
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

const UpdateBook = async (token, id, data) => {
    try {
        const response = await axios.put(`${API_URL}/UpdateBook/${id}`, data, {
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

const DeleteBook = async (token, id) => {
    try {
        const response = await axios.delete(`${API_URL}/DeleteBook/${id}`, {
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
    GetBooks,
    GetTopBooks,
    SearchBooks,
    GetBookById,
    GetNewInfoBooks,
    GetRelatedBooks,
    CreateBook,
    UploadImage,
    UpdateBook,
    DeleteBook
};
