import axios from 'axios';

const API_URL = 'https://localhost:7138/api/Payment';

const CreatePayment = async (data) => {
    try {
        const response = await axios.post(`${API_URL}/CreatePayment`, data);
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
    CreatePayment
}