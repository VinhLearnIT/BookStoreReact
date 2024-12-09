import { RefreshToken } from '../services/AuthService';

const refreshToken = async () => {
    try {
        const refreshToken = localStorage.getItem('refreshToken');
        const response = await RefreshToken({ refreshToken });
        if (response.status === 200) {
            localStorage.setItem('accessToken', response.data.accessToken);
            return true;
        } else {
            return false;
        }
    } catch (error) {
        return false;
    }
};

export default refreshToken;