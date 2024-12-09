import axios from 'axios';

const API_URL = 'https://localhost:7138/api/OrderDetail';

const GetOrderDetailByOrderID = async (token, id) => {
    try {
        const response = await axios.get(`${API_URL}/GetOrderDetailByOrderID/${id}`, {
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
    GetOrderDetailByOrderID
};
