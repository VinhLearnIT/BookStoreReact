import React, { useState, useCallback, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { App, Breadcrumb, Table, Input, Popconfirm, Tooltip, Modal, Form, Button, Select } from 'antd';
import { SearchOutlined, EditOutlined, CloseOutlined, EyeOutlined, QuestionCircleOutlined } from '@ant-design/icons';
import OrderDetailModal from '../../components/OrderDetailModal';
import * as ordersService from '../../services/OrdersService';
import * as orderDetailService from '../../services/OrderDetailService';
import refreshToken from '../../utils/RefreshToken';
const ManageOrder = () => {
    const navigate = useNavigate();
    const { message } = App.useApp();
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchText, setSearchText] = useState('');
    const [selectOptions, setSelectOptions] = useState([]);
    const [isModalDetailOpen, setIsModalDetailOpen] = useState(false);
    const [isModalStatusOpen, setIsModalStatusOpen] = useState(false);
    const [form] = Form.useForm();
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
            let token = localStorage.getItem('accessToken');
            let response = await ordersService.GetOrders(token);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await ordersService.GetOrders(token);
            }

            if (response.status === 200) {
                setOrders(response.data);
            }
        } catch (error) {
            message.error("Không thể tại dữ liệu đơn hàng!");
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
                message.error("Không có chi tiết!");
            }
        } catch (error) {
            message.error("Không thể tải danh sách!");
            console.log(error);
        }
    }

    const filteredOrders = orders.filter(order => {
        var stringSearch = searchText.toLowerCase();
        return String(order.orderID) === stringSearch ||
            order.fullName.toLowerCase().includes(stringSearch) ||
            order.orderDate.toLowerCase().includes(stringSearch)
    });

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
                message.error("Không thể cập nhật trạng thái đơn hàng!");
            }
        } catch (error) {
            message.error("Lỗi khi cập nhật trạng thái đơn hàng!");
            console.log(error);
        }
        setIsModalStatusOpen(false);
        selectOrder.current = null
    }

    const handleSubmit = async (values) => {
        if (selectOrder.current) {
            handleUpdateStatus(selectOrder.current, values.orderStatus);
        }
    }

    const handleOpenModalStatus = (order) => {
        setIsModalStatusOpen(true);
        selectOrder.current = order;
        form.setFieldsValue({
            orderStatus: order.orderStatus
        });
        const nextStatusOptions = getNextStatusOptions(order.orderStatus);
        setSelectOptions(nextStatusOptions);
    };

    const handleCancelModalStatus = () => {
        setIsModalStatusOpen(false);
        selectOrder.current = null;
    }

    const getNextStatusOptions = (currentStatus) => {
        switch (currentStatus) {
            case 'Pending':
                return [
                    { label: 'Chờ xác nhận', value: 'Pending', disabled: true },
                    { label: 'Đang vận chuyển', value: 'Shipped', disabled: false },
                    { label: 'Hoàn thành', value: 'Completed', disabled: false },
                ];
            case 'Shipped':
                return [
                    { label: 'Chờ xác nhận', value: 'Pending', disabled: true },
                    { label: 'Đang vận chuyển', value: 'Shipped', disabled: true },
                    { label: 'Hoàn thành', value: 'Completed', disabled: false },
                ];
            default:
                return [];
        }
    };

    const statusMap = {
        Pending: <span className="text-yellow-500 font-medium">Chờ xác nhận</span>,
        Shipped: <span className="text-blue-500 font-medium">Đang vận chuyển</span>,
        Completed: <span className="text-green-500 font-medium">Hoàn thành</span>,
        Cancelled: <span className="text-red-400 font-medium">Đã hủy</span>,
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
                <Tooltip placement="topLeft" title={new Date(orderDate).toLocaleDateString()}>
                    {new Date(orderDate).toLocaleDateString()}
                </Tooltip>
            ),
        },
        {
            title: 'Thanh toán', dataIndex: 'paymentMethod', key: 'paymentMethod', align: 'center', ellipsis: true, width: 180,
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
                const isEditStatus = record.orderStatus === 'Completed' || record.orderStatus === 'Cancelled';

                return (
                    <div className='flex gap-2 justify-center'>
                        <Tooltip title="Cập nhật trạng thái đơn hàng" placement='bottom' color={"gold"}>
                            <Button
                                className={`px-3 py-5 ${isEditStatus ? '!border-gray-400' : 'border-yellow-500 hover:!border-yellow-500'}`}
                                disabled={isEditStatus}
                                onClick={() => {
                                    handleOpenModalStatus(record);
                                }}>
                                <EditOutlined className={isEditStatus ? 'text-gray-400' : 'text-yellow-500'} />
                            </Button>
                        </Tooltip>

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
                                    <CloseOutlined />
                                </Button>
                            </Tooltip>
                        </Popconfirm>
                    </div>
                );
            }
        }
    ];
    return (
        <>
            <Breadcrumb
                items={[
                    { title: 'Admin' },
                    { title: "Quản lý đơn hàng" }
                ]}
            />
            <div className='bg-white mt-4 p-4 rounded-md shadow-md' style={{ minHeight: 'calc(100vh - 8rem)' }}>
                <Input
                    placeholder="Tìm đơn hàng..."
                    prefix={<SearchOutlined className='mr-2 ' />}
                    onChange={(e) => setSearchText(e.target.value)}
                    className='mb-4'
                    style={{ width: 400 }}
                />

                <Table
                    columns={columns}
                    dataSource={filteredOrders}
                    rowKey="orderID"
                    loading={loading}
                    pagination={{ pageSize: 6 }}
                />
            </div>
            <Modal
                open={isModalStatusOpen}
                onCancel={handleCancelModalStatus}
                footer={null}
                width={350}
                style={{ top: 150 }}
            >
                <p className='text-center text-xl text-custom1 font-bold mb-6'>CẬP NHẬT TRẠNG THÁI </p>
                <Form form={form} onFinish={handleSubmit} layout="horizontail" requiredMark={false}>
                    <Form.Item
                        name="orderStatus"
                        label="Trạng thái"
                        rules={[{ required: true, message: 'Vui lòng chọn trạng thái!' }]}
                    >
                        <Select
                            placeholder="Chọn trạng thái"
                            className='mb-1 mt-2'
                            options={selectOptions}
                        />
                    </Form.Item>
                    <Form.Item>
                        <Button type='primary' htmlType="submit" className="w-full h-10 mt-2">
                            Cập nhật
                        </Button>
                    </Form.Item>
                </Form>
            </Modal>

            <OrderDetailModal
                isModalOpen={isModalDetailOpen}
                order={selectOrder.current}
                orderDetail={orderDetail.current}
                onClose={() => {
                    setIsModalDetailOpen(false);
                    selectOrder.current = null;
                }}
            />
        </>
    )
}

export default ManageOrder;