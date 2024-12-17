import React, { useState, useCallback, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { App, Table, Popconfirm, Tooltip, Button } from 'antd';
import { CloseOutlined, EyeOutlined, QuestionCircleOutlined } from '@ant-design/icons';
import OrderDetailModal from '../../components/OrderDetailModal';
import * as ordersService from '../../services/OrdersService';
import * as orderDetailService from '../../services/OrderDetailService';
import refreshToken from '../../utils/RefreshToken';

const Order = () => {
    const navigate = useNavigate();
    const { message } = App.useApp();
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [isModalDetailOpen, setIsModalDetailOpen] = useState(false);
    const orderDetail = useRef([]);
    const selectOrder = useRef(null);

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

    const getOrders = useCallback(async () => {
        try {
            let customerID = localStorage.getItem('customerID');
            let token = localStorage.getItem('accessToken');
            let response = await ordersService.GetOrderByCustomerId(token, customerID);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await ordersService.GetOrderByCustomerId(token, customerID);
            }

            if (response.status === 200) {
                setOrders(response.data);
            } else {
                message.error(response.message);
            }
        } catch (error) {
            message.error("Không thể tải dữ liệu!");
            console.log(error);
        } finally {
            setLoading(false);
        }
    }, [message, refreshAccessToken]);

    useEffect(() => {
        getOrders();
    }, [getOrders]);

    const handleViewOrderDetail = async (order) => {
        try {
            let token = localStorage.getItem('accessToken');
            let response = await orderDetailService.GetOrderDetailByOrderID(token, order.orderID);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await orderDetailService.GetOrderDetailByOrderID(token, order.orderID);
            }

            if (response.status === 200) {
                orderDetail.current = response.data
                setIsModalDetailOpen(true);
            } else if (response.status === 404) {
                message.warning("Không có chi tiết!");
            }
        } catch (error) {
            message.error("Không thể tải dữ liệu!");
            console.log(error);
        }
    }
    const handleUpdateStatus = async (order, status) => {
        try {
            let token = localStorage.getItem('accessToken');
            let response = await ordersService.UpdateOrderStatus(token, order.orderID, { orderStatus: status });

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await ordersService.UpdateOrderStatus(token, order.orderID, { orderStatus: status });
            }

            if (response.status === 200) {
                message.success("Cập nhật trạng thái đơn hàng thành công!");
                await getOrders();
            } else {
                message.error(response.message);
            }
        } catch (error) {
            message.error("Không thể cập nhật trạng thái đơn hàng!");
            console.log(error);
        }
    }

    const statusMap = {
        Pending: <span className="text-yellow-500 font-medium">Chờ xử lý</span>,
        Processing: <span className="text-blue-600 font-medium">Đang xử lý</span>,
        Shipped: <span className="text-indigo-700 font-medium">Đang vận chuyển</span>,
        Completed: <span className="text-green-500 font-medium">Hoàn thành</span>,
        Cancelled: <span className="text-red-500 font-medium">Đã hủy</span>,
    };

    const columns = [
        { title: 'ID', dataIndex: 'orderID', key: 'orderID', align: 'center', width: 80 },
        {
            title: 'Người mua', dataIndex: 'fullName', key: 'fullName', align: 'center', ellipsis: true,
            render: (fullName) => (
                <Tooltip placement="topLeft" title={fullName}>
                    {fullName}
                </Tooltip>
            ),
        },
        {
            title: 'Địa chỉ', dataIndex: 'address', key: 'address', align: 'center', ellipsis: true,
            render: (address) => (
                <Tooltip placement="topLeft" title={address}>
                    {address}
                </Tooltip>
            ),
        },
        {
            title: 'Ngày đặt hàng', dataIndex: 'orderDate', key: 'orderDate', align: 'center', ellipsis: true, width: 150,
            render: (orderDate) => (
                <Tooltip placement="topLeft" title={new Date(orderDate)?.toLocaleDateString()}>
                    {new Date(orderDate)?.toLocaleDateString()}
                </Tooltip>
            ),
        },
        {
            title: 'Thanh toán', dataIndex: 'paymentMethod', key: 'paymentMethod', align: 'center', ellipsis: true, width: 200,
            render: (paymentMethod) => (
                <Tooltip placement="topLeft" title={paymentMethod === "COD" ? "Khi nhận hàng" : "Thanh toán qua MoMo"}>
                    {paymentMethod === "COD" ? "Khi nhận hàng" : "Thanh toán qua MoMo"}
                </Tooltip>
            ),
        },
        {
            title: 'Trạng thái', dataIndex: 'orderStatus', key: 'orderStatus', align: 'center', width: 180,
            render: (orderStatus) => (statusMap[orderStatus]),
        },
        {
            title: 'Thao tác', key: 'actions', align: 'center', width: 200,
            render: (_, record) => {
                const isCancel = record.orderStatus !== 'Pending';
                return (
                    <div className='flex gap-2 justify-center'>
                        <Tooltip title="Xem chi tiết" placement='bottom' color={"blue"}>
                            <Button className="px-3 py-5" onClick={() => {
                                selectOrder.current = record;
                                handleViewOrderDetail(record);
                            }} >
                                <EyeOutlined className='text-blue-600' />
                            </Button>
                        </Tooltip>
                        <Popconfirm
                            icon={<QuestionCircleOutlined style={{ color: 'red' }} />}
                            title="Xác nhận hủy đơn hàng?"
                            okText="Xác nhận"
                            cancelText="Hủy"
                            onConfirm={() => handleUpdateStatus(record, "Cancelled")}>
                            <Tooltip title="Hủy đơn hàng" placement='bottom' color={"red"}>
                                <Button
                                    danger
                                    className={`px-3 py-5 ${isCancel ? '!border-gray-400' : ''}`}
                                    disabled={isCancel}
                                >
                                    <CloseOutlined className='text-lg' />
                                </Button>
                            </Tooltip>
                        </Popconfirm>
                    </div>
                );
            }
        }
    ]

    return (
        <div className='max-w-screen-xl mx-auto mt-10'>
            <h2 className="text-3xl font-bold text-custom1 mb-10 text-center ">
                THÔNG TIN ĐƠN HÀNG
            </h2>
            <Table
                columns={columns}
                dataSource={orders}
                rowKey="orderID"
                loading={loading}
                pagination={{ pageSize: 6 }}
            />
            <OrderDetailModal
                isModalOpen={isModalDetailOpen}
                order={selectOrder.current}
                orderDetail={orderDetail.current}
                onClose={() => {
                    setIsModalDetailOpen(false);
                    selectOrder.current = null;
                }}
            />
        </div>
    )
}

export default Order;