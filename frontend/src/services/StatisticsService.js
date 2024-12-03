import axios from 'axios';

const API_URL = 'https://localhost:7138/api/Statistics';

const GetMonthlyStatistics = async (token) => {
    try {
        const response = await axios.get(`${API_URL}/GetMonthlyStatistics`, {
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

const GetMonthlyRevenueOfYear = async (token, year) => {
    try {
        const response = await axios.get(`${API_URL}/GetMonthlyRevenueOfYear/${year}`, {
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


const GetMonthlySalesOfYear = async (token, year) => {
    try {
        const response = await axios.get(`${API_URL}/GetMonthlySalesOfYear/${year}`, {
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
    GetMonthlyStatistics,
    GetMonthlyRevenueOfYear,
    GetMonthlySalesOfYear
};
