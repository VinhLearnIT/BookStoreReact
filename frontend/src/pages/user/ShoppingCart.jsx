import React, { useState, useCallback, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { Form, Input, Button, Radio, Table, App, InputNumber, Popconfirm, Tooltip } from 'antd';
import { DeleteOutlined, QuestionCircleOutlined } from '@ant-design/icons';
import * as shoppingCartService from '../../services/ShoppingCartService';
import { CreatePayment } from '../../services/PaymentService';
import { GetNewInfoBooks } from '../../services/BookService';
import refreshToken from '../../utils/RefreshToken';
import { CartContext } from '../../components/CartContext';

import MoMo from '../../assets/images/momo.jpg'
import COD from '../../assets/images/COD.png'
const ShoppingCart = () => {
    const navigate = useNavigate();
    const { removeFromCart } = useContext(CartContext);
    const { message } = App.useApp();
    const [form] = Form.useForm();
    const [paymentMethod, setPaymentMethod] = useState('COD');
    const [shoppingCart, setShoppingCart] = useState([]);
    const isAuthenticated = localStorage.getItem('isAuthenticated') === 'true';
    const expireTime = localStorage.getItem('expireTime');
    const currentTime = new Date();
    const isLogin = isAuthenticated && expireTime && new Date(expireTime) > currentTime;

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

    // Start ---- Xử lý lấy thông tin giỏ hàng
    const getCartForCustomer = useCallback(async () => {
        try {
            let customerID = localStorage.getItem('customerID');
            let token = localStorage.getItem('accessToken');
            let response = await shoppingCartService.GetCartByCustomerId(token, customerID);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await shoppingCartService.GetCartByCustomerId(token, customerID);
            }

            if (response.status === 200) {
                setShoppingCart(response.data);
            } else {
                message.error(response.message || "Có lỗi trong quá trình tải dữ liệu");
            }

        } catch (error) {
            message.error("Không thể tải dữ liệu!");
            console.log(error);
        }
    }, [message, refreshAccessToken]);

    const getCartForGuest = useCallback(async () => {
        try {
            let cart = JSON.parse(localStorage.getItem('cart')) || [];
            if (cart.length > 0) {
                const arrID = cart.map(c => c.bookID).join(', ');
                const response = await GetNewInfoBooks(arrID);
                if (response.status === 200) {
                    const dataNew = response.data;
                    cart = cart.map(c => {
                        const updatedBook = dataNew.find(book => book.bookID === c.bookID);
                        if (updatedBook) {
                            return {
                                ...c,
                                bookName: updatedBook.bookName,
                                imagePath: updatedBook.imagePath,
                                price: updatedBook.price,
                            };
                        }
                        return c;
                    });
                } else {
                    message.error(response.message || "Có lỗi trong quá trình tải dữ liệu");
                }
            }
            setShoppingCart(cart);
        } catch (error) {
            message.error("Không thể tải dữ liệu!");
            console.log(error);
        }
    }, [message]);

    const getShoppingCart = useCallback(async () => {
        try {
            if (isLogin) {
                await getCartForCustomer();
            } else {
                await getCartForGuest();
            }
        } catch (error) {
            message.error("Không thể tải dữ liệu!");
            console.log(error);
        }
    }, [message, isLogin, getCartForCustomer, getCartForGuest]);

    useEffect(() => {
        getShoppingCart();
    }, [getShoppingCart]);

    const calculateTotal = () => {
        return shoppingCart.reduce((total, item) => total + (item.quantity * item.price), 0);
    };
    const totalAmount = calculateTotal();
    // End ---- Kết thúc xử lí lấy thông tin giỏ hàng

    // Start ---- Xử lí cập nhật số lượng giỏ hàng
    const handleUpdateQuantity = async (cart, newQuantity) => {
        try {
            const stockCheckResponse = await shoppingCartService.CheckStockQuantity(cart.bookID, newQuantity);
            if (stockCheckResponse.status !== 200) {
                message.error(stockCheckResponse.message || "Có lỗi xảy ra khi kiểm tra số lượng!");
                return;
            }
            if (isLogin) {
                await updateCartForCustomer(cart, newQuantity);
            } else {
                updateCartForGuest(cart, newQuantity);
            }
        } catch (error) {
            message.error("Lỗi khi cập nhật số lượng sản phẩm!");
            console.error(error);
        }
    }
    const updateCartForCustomer = async (cart, newQuantity) => {
        try {
            const cartData = {
                bookID: cart.bookID,
                quantity: newQuantity
            };
            let token = localStorage.getItem('accessToken');
            let response = await shoppingCartService.UpdateCart(token, cart.cartID, cartData);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await shoppingCartService.UpdateCart(token, cart.cartID, cartData);
            }

            if (response.status === 200) {
                await getShoppingCart();
                message.success("Cập nhật số lượng thành công!");
            } else {
                message.error(response.message || "Lỗi khi cập nhật số lượng");
            }
        } catch (error) {
            message.error("Lỗi khi cập nhật số lượng");
            console.log(error);
        }

    }
    const updateCartForGuest = (cart, newQuantity) => {
        const updatedCart = shoppingCart.map(item =>
            item.cartID === cart.cartID
                ? { ...item, quantity: newQuantity }
                : item
        );
        setShoppingCart(updatedCart);
        localStorage.setItem('cart', JSON.stringify(updatedCart));
        message.success("Cập nhật số lượng thành công!");
    };
    // End ---- Xử lí cập nhật số lượng giỏ hàng

    // Start ---- Xử lí xóa giỏ hàng
    const handleDeleteCart = async (cart) => {
        try {
            if (isLogin) {
                await deleteCartForCustomer(cart.cartID);
            } else {
                deleteCartForGuest(cart.cartID);
            }
        } catch (error) {
            message.error("Lỗi khi xóa sản phẩm!");
            console.error(error);
        }
    }
    const deleteCartForCustomer = async (cartID) => {
        try {

            let token = localStorage.getItem('accessToken');
            let response = await shoppingCartService.DeleteCart(token, cartID);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await shoppingCartService.DeleteCart(token, cartID);
            }

            if (response.status === 200) {
                await getShoppingCart();
                message.success("Xóa sản phẩm thành công!");
                removeFromCart(1);
            } else {
                message.error(response.message || "Lỗi khi xóa sản phẩm");
            }
        } catch (error) {
            message.error("Lỗi khi xóa sản phẩm");
            console.log(error);
        }

    }
    const deleteCartForGuest = (cartID) => {
        const updatedCart = shoppingCart.filter(item =>
            item.cartID !== cartID
        );
        setShoppingCart(updatedCart);
        localStorage.setItem('cart', JSON.stringify(updatedCart));
        removeFromCart(1);
        message.success("Xóa sản phẩm thành công!");
    }
    // End ---- Xử lí xóa giỏ hàng

    // Start ---- Xử lí thanh toán giỏ hàng
    const handlePaymentOrder = async (formData) => {
        try {
            if (!isLogin) {
                const orderInfo = `Thanh toán đơn hàng ${formData.fullName}`;
                await createPaymentRequest(orderInfo);
                localStorage.setItem('infoGuest', JSON.stringify(formData));
            } else if (isLogin && paymentMethod === "MOMO") {
                const customerID = localStorage.getItem('customerID');
                const orderInfo = `Thanh toán đơn hàng ${customerID}`;
                await createPaymentRequest(orderInfo);
            } else {
                sessionStorage.setItem('isPayment', true);
                navigate('/payment');
            }
        } catch (error) {
            message.error("Lỗi khi xóa sản phẩm!");
            console.error(error);
        }
    }
    const createPaymentRequest = async (orderInfo) => {
        try {
            const amount = shoppingCart.reduce((amount, item) => amount + (item.quantity * item.price), 0);
            const requestData = { orderInfo, amount };
            console.log(requestData);
            const response = await CreatePayment(requestData);

            if (response.data && response.data.paymentUrl) {
                sessionStorage.setItem('isPayment', true);
                window.location.href = response.data.paymentUrl;
            } else {
                message.error("Lỗi khi tạo thanh toán cho người dùng");
            }
        } catch (error) {
            message.error("Có lỗi xảy ra khi tạo thanh toán.");
        }
    };

    // End ---- Xử lí thanh toán giỏ hàng


    const columns = [
        {
            title: 'Ảnh', dataIndex: 'imagePath', key: 'imagePath', align: 'center', width: 100,
            render: (path) =>
                <img
                    src={`https://localhost:7138/api/images/${path}`}
                    alt="Book"
                    className='w-full h-12 object-cover rounded-md border border-custom1 p-1' />
        },
        {
            title: 'Tên sản phẩm', dataIndex: 'bookName', key: 'bookName', ellipsis: true,
            render: (bookName) => (
                <Tooltip placement="topLeft" title={bookName}>
                    {bookName}
                </Tooltip>
            ),
        },
        {
            title: 'Số lượng', dataIndex: 'quantity', key: 'quantity', align: 'center', width: 100,
            render: (_, record) => (
                <InputNumber
                    min={1}
                    defaultValue={record.quantity}
                    className='mb-1 mt-2  w-full'
                    onBlur={(e) => {
                        let newQuantity = parseInt(e.target.value);
                        if (isNaN(newQuantity) || newQuantity <= 0) {
                            message.error("Vui lòng nhập số lượng!");
                            return
                        }
                        if (newQuantity !== record.quantity)
                            handleUpdateQuantity(record, newQuantity);
                    }}
                    placeholder='Nhập số lượng' />
            ),
        },
        {
            title: 'Giá', dataIndex: 'price', key: 'price', align: 'center', width: 120,
            render: price => `${price?.toLocaleString()} VNĐ`
        },
        {
            title: 'Tổng', key: 'total', align: 'center', width: 120,
            render: (_, record) => `${(record.quantity * record.price)?.toLocaleString()} VNĐ`
        },
        {
            title: 'Thao tác', key: 'actions', align: 'center', width: 100,
            render: (_, record) => {

                return (
                    <div className='flex gap-2 justify-center'>
                        <Popconfirm
                            icon={<QuestionCircleOutlined style={{ color: 'red' }} />}
                            title="Xác nhận xóa sản phẩm?"
                            okText="Xác nhận"
                            cancelText="Hủy"
                            onConfirm={() => handleDeleteCart(record)}>
                            <Tooltip title="Xóa sản phẩm" placement='bottom' color={"red"}>
                                <Button danger className='px-3 py-5'>
                                    <DeleteOutlined className='text-lg' />
                                </Button>
                            </Tooltip>
                        </Popconfirm>
                    </div>
                );
            }
        }
    ];



    return (
        <div className="max-w-screen-xl mx-auto mt-14 p-6 rounded-md shadow grid grid-cols-1 md:grid-cols-[7fr_3fr] gap-10 border border-custom1">
            <div>
                <h2 className="text-2xl font-bold mb-6 text-center text-custom1">GIỎ HÀNG CỦA BẠN</h2>
                <Table dataSource={shoppingCart} columns={columns} pagination={false} rowKey="cartID" />
                <div className="text-right font-bold mt-6 text-lg">
                    <span>Tổng số tiền: </span>
                    <span>{totalAmount?.toLocaleString()} VNĐ</span>
                </div>
            </div>

            <div>
                <h2 className="text-2xl font-bold mb-6 text-center text-custom1">THÔNG TIN THANH TOÁN</h2>
                <Form form={form} layout="vertical" onFinish={handlePaymentOrder}>
                    {!isLogin && (
                        <>
                            <Form.Item
                                label="Họ và tên"
                                name="fullName"
                                rules={[{ required: true, message: 'Vui lòng nhập họ và tên!' }]}>
                                <Input />
                            </Form.Item>
                            <Form.Item
                                label="Email"
                                name="email"
                                rules={[
                                    { required: true, message: 'Vui lòng nhập email hợp lệ!' },
                                    { type: 'email', message: 'Email không hợp lệ!' }
                                ]}>
                                <Input />
                            </Form.Item>
                            <Form.Item
                                label="Số điện thoại"
                                name="phone"
                                rules={[
                                    { required: true, message: 'Vui lòng nhập số điện thoại!' },
                                    { pattern: /^[0-9]{10}$/, message: 'Số điện thoại là 10 số' }
                                ]}>
                                <Input maxLength={10} />
                            </Form.Item>
                            <Form.Item
                                label="CCCD"
                                name="cccd"
                                rules={[
                                    { required: true, message: 'Vui lòng nhập CCCD!' },
                                    { pattern: /^[0-9]{12}$/, message: 'Căn cước công dân là 12 số' }
                                ]}>
                                <Input maxLength={12} />
                            </Form.Item>
                            <Form.Item
                                label="Địa chỉ"
                                name="address"
                                rules={[{ required: true, message: 'Vui lòng nhập địa chỉ!' }]}>
                                <Input.TextArea rows={3} />
                            </Form.Item>
                        </>
                    )}
                    <Form.Item label="Phương thức thanh toán" className="mt-4">
                        <Radio.Group
                            onChange={(e) => setPaymentMethod(e.target.value)}
                            value={isLogin ? paymentMethod : "MOMO"}>
                            {isLogin ? (
                                <>
                                    <Radio value="COD" className='mt-2'>
                                        <div className='flex items-center'>
                                            Thanh toán khi nhận hàng
                                            <img src={COD} alt="COD" className='ml-2 w-24' />
                                        </div>
                                    </Radio>
                                    <Radio value="MOMO" className='mt-2'>
                                        <div className='flex items-center'>
                                            Thanh toán qua MoMo
                                            <img src={MoMo} alt="MôM" className='ml-2 h-10 rounded-md' />
                                        </div>
                                    </Radio>
                                </>
                            ) : (
                                <Radio value="MOMO" className='mt-2'>
                                    <div className='flex items-center'>
                                        Thanh toán qua MoMo
                                        <img src={MoMo} alt="MôM" className='ml-2 h-10 rounded-md' />
                                    </div>
                                </Radio>
                            )}
                        </Radio.Group>
                    </Form.Item>

                    <Form.Item className="mt-6">
                        <Button type="primary" htmlType="submit" className="w-full" disabled={totalAmount === 0}>
                            Đặt hàng
                        </Button>
                    </Form.Item>
                </Form>
            </div>
        </div>
    );
};

export default ShoppingCart;
