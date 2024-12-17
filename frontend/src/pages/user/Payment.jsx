import React, { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, App } from 'antd';
import * as ordersService from '../../services/OrdersService';
import refreshToken from '../../utils/RefreshToken';
import { CloseCircleOutlined, CheckCircleOutlined } from '@ant-design/icons';
import { CartContext } from '../../components/CartContext';

const Payment = () => {
    const { message } = App.useApp();
    const { cartCount, setCartQuantity } = useContext(CartContext);
    const navigate = useNavigate();
    const isAuthenticated = localStorage.getItem('isAuthenticated') === 'true';
    const expireTime = localStorage.getItem('expireTime');
    const currentTime = new Date();
    const isLogin = isAuthenticated && expireTime && new Date(expireTime) > currentTime;
    const queryParams = new URLSearchParams(window.location.search);
    const resultCode = queryParams.get("resultCode") || "0";
    const partnerCode = queryParams.get("partnerCode");
    const [isSuccess, setIsSuccess] = useState(true); // Trạng thái kiểm tra API

    const refreshAccessToken = useCallback(async () => {
        try {
            const refreshTokenBolean = await refreshToken();
            if (!refreshTokenBolean) {
                message.error("Phiên đăng nhập của bạn đã hết hạn!");
                localStorage.setItem('isAuthenticated', false);
                navigate("/auth/login");
                return null;
            }
            return localStorage.getItem('accessToken');
        } catch (error) {
            console.error("Lỗi làm mới token:", error);
            message.error("Không thể làm mới phiên đăng nhập!");
            return null;
        }
    }, [message, navigate]);

    const orderForCustomer = useCallback(async () => {
        try {
            let customerID = localStorage.getItem('customerID');
            let token = localStorage.getItem('accessToken');
            const data = {
                customerID,
                orderStatus: partnerCode === "MOMO" ? "Shipped" : "Processing",
                paymentMethod: partnerCode === "MOMO" ? "MOMO" : "COD",
            }
            let response = await ordersService.AddOrderForCustomer(token, data);
            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await ordersService.AddOrderForCustomer(token, data);
            }

            if (response.status === 200) {
                setCartQuantity(0);
                message.success("Thanh toán thành công!");
            } else {
                setIsSuccess(false);
                message.error(response.message || "Có lỗi trong quá trình thanh toán!");
            }
        } catch (error) {
            message.error("Có lỗi khi thanh toán!");
            setIsSuccess(false);
            console.log(error);
        }
    }, [message, refreshAccessToken, partnerCode, setCartQuantity]);

    const orderForGuest = useCallback(async () => {
        try {
            let infoGuest = JSON.parse(localStorage.getItem('infoGuest')) || {};
            let cart = JSON.parse(localStorage.getItem('cart')) || [];
            const guestOrder = {
                ...infoGuest,
                cartItems: cart.map(item => ({
                    bookID: item.bookID,
                    quantity: item.quantity,
                    price: item.price
                })),
                paymentMethod: "MOMO"
            };
            let response = await ordersService.AddOrderForGuest(guestOrder);

            if (response.status === 200) {
                setCartQuantity(0);
                localStorage.setItem('cart', JSON.stringify([]));
                message.success("Thanh toán thành công!");
            } else {
                message.error(response.message || "Có lỗi trong quá trình thanh toán!");
                setIsSuccess(false);
            }
        } catch (error) {
            message.error("Có lỗi khi thanh toán!");
            setIsSuccess(false);
            console.log(error);
        }
    }, [message, setCartQuantity]);

    useEffect(() => {
        const isPayment = JSON.parse(sessionStorage.getItem('isPayment'));
        if (cartCount > 0 && isPayment && resultCode === "0") {
            sessionStorage.setItem('isPayment', false);
            if (!isLogin) {
                orderForGuest();
            } else {
                orderForCustomer();
            }
        }
    }, [resultCode, cartCount, isLogin, orderForCustomer, orderForGuest]);

    return (
        <div className='max-w-screen-xl mx-auto mt-10 border border-custom1 rounded-md h-[65vh]'>
            {resultCode === "0" && isSuccess ? (
                <div className="flex flex-col justify-center items-center w-1/2 h-full mx-auto gap-4 ">
                    <CheckCircleOutlined className='text-9xl text-green-600' />
                    <h2 className="text-2xl font-bold text-green-600">Đặt hàng thành công!</h2>
                    <p>Cảm ơn bạn đã đặt hàng. Đơn hàng của bạn sẽ được xử lý sớm.</p>
                    <Button type="primary" className='h-9' onClick={() => navigate('/home')}>
                        Quay lại Trang chủ
                    </Button>
                </div>
            ) : (
                <div className="flex flex-col justify-center items-center w-1/2 h-full mx-auto gap-4">
                    <CloseCircleOutlined className='text-9xl text-red-600' />
                    <h2 className="text-2xl font-bold text-red-600">Đặt hàng thất bại!</h2>
                    <p>Rất tiếc, đã xảy ra lỗi trong quá trình đặt hàng. Vui lòng thử lại.</p>
                    <Button type="primary" className='h-9' onClick={() => navigate('/shopping-cart')}>
                        Quay lại Giỏ hàng
                    </Button>
                </div>
            )}
        </div>
    )
}

export default Payment;
