import React, { useState, useCallback, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { App, Breadcrumb, Table, Input, Popconfirm, Tooltip, Modal, Form, Button, Select } from 'antd';
import { SearchOutlined, EditOutlined, CloseOutlined, CheckOutlined, QuestionCircleOutlined }
    from '@ant-design/icons';
import * as customerService from '../../services/CustomerService';
import refreshToken from '../../utils/RefreshToken';
const ManageUser = () => {
    const navigate = useNavigate();
    const { message } = App.useApp();
    const [customer, setCustomer] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchText, setSearchText] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [form] = Form.useForm();

    const selectCustomerID = useRef(null);
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

    const getCustomers = useCallback(async () => {
        try {
            let token = localStorage.getItem('accessToken');
            let response = await customerService.GetCustomers(token);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await customerService.GetCustomers(token);
            }

            if (response.status === 200) {
                setCustomer(response.data);
            }
        } catch (error) {
            message.error("Không thể tải dữ liệu người dùng!");
            console.log(error);
        } finally {
            setLoading(false);
        }
    }, [message, refreshAccessToken]);

    useEffect(() => {
        getCustomers();
    }, [getCustomers]);

    const filteredCustomer = customer.filter(c => {
        var stringSearch = searchText.toLowerCase();
        return String(c.customerID) === stringSearch || c.fullName.toLowerCase().includes(stringSearch)
    });

    const handleOpenModal = (customer) => {
        setIsModalOpen(true);
        selectCustomerID.current = customer.customerID;
        form.setFieldsValue({ role: customer.role });
    };

    const handleCancelModal = () => {
        setIsModalOpen(false);
        selectCustomerID.current = null
        form.resetFields();
    };

    const handleSubmit = async (values) => {
        try {
            let token = localStorage.getItem('accessToken');
            let response = await customerService.UpdateCustomerRole(token, selectCustomerID.current, { role: values.role });

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await customerService.UpdateCustomerRole(token, selectCustomerID.current, { role: values.role });
            }

            if (response.status === 200) {
                message.success("Cập nhật chức năng người dùng thành công!");
                await getCustomers();
            } else {
                message.error("Không thể cập nhật chức năng người dùng!");
            }
        } catch (error) {
            message.error("Lỗi cập nhật chức năng người dùng!");
            console.log(error);
        }
        setIsModalOpen(false);
        selectCustomerID.current = null
    }

    const handleUpdateCustomerStatus = async (status) => {
        try {
            let token = localStorage.getItem('accessToken');
            let response = await customerService.UpdateCustomerStatus(token, selectCustomerID.current, { isDeleted: status });

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await customerService.UpdateCustomerStatus(token, selectCustomerID.current, { isDeleted: status });
            }
            if (response.status === 200) {
                if (status) {
                    message.success("Vô hiệu hóa người dùng thành công!");
                } else {
                    message.success("Kích hoạt người dùng thành công!");
                }
                await getCustomers();
            }
        } catch (error) {
            if (status) {
                message.error("Không thể vô hiệu hóa người dùng!");
            } else {
                message.error("Không thể kích hoạt người dùng!");
            }
            console.log(error);
        }
    };

    const maskData = (data, type) => {
        if (!data) return '';
        let firstPart, lastPart;
        switch (type) {
            case 'email':
                firstPart = data.split('@')[0].slice(0, 3);
                lastPart = data.split('@')[0].slice(-2);
                return firstPart + '***' + lastPart + '@' + data.split('@')[1];
            case 'cccd':
                firstPart = data.slice(0, 4);
                lastPart = data.slice(-4);
                return firstPart + '****' + lastPart;
            default:
                return data;
        }
    };

    const roleMap = {
        Manager: 'Quản trị viên',
        Admin: 'Quản lý',
        User: 'Người dùng',
    };

    const columns = [
        { title: 'ID', dataIndex: 'customerID', key: 'customerID', align: 'center', width: 50 },
        {
            title: 'Họ và tên', dataIndex: 'fullName', key: 'fullName', align: 'center', ellipsis: true, width: 150,
            render: (fullName) => (
                <Tooltip placement="topLeft" title={fullName}>
                    {fullName}
                </Tooltip>
            ),
        },
        {
            title: 'Email', dataIndex: 'email', key: 'email', align: 'center', ellipsis: true,
            render: (email) => (
                <Tooltip placement="topLeft" title={maskData(email, "email")}>
                    {maskData(email, "email")}
                </Tooltip>
            ),
        },
        {
            title: 'Số điện thoại', dataIndex: 'phone', key: 'phone', align: 'center', ellipsis: true,
            render: (phone) => (
                <Tooltip placement="topLeft" title={phone}>
                    {phone}
                </Tooltip>
            ),
        },
        {
            title: 'CCCD', dataIndex: 'cccd', key: 'cccd', align: 'center', ellipsis: true,
            render: (cccd) => (
                <Tooltip placement="topLeft" title={maskData(cccd, "cccd")}>
                    {maskData(cccd, "cccd")}
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
            title: 'Thông tin', dataIndex: 'fullInfo', key: 'fullInfo', align: 'center',
            render: (text) => text ? "Đầy đủ" : "Chưa cập nhật",
            filters: [
                { text: 'Đầy đủ ', value: true },
                { text: 'Chưa cập nhật', value: false },
            ],
            onFilter: (value, record) => record.fullInfo === value,
            filterSearch: true,
        },
        {
            title: 'Chức năng', dataIndex: 'role', key: 'role', align: 'center',
            render: (role) => (
                <span className={`${role === "Manager" ? "text-purple-800" : role === "Admin" ? "text-violet-600" : "text-blue-500"} font-medium`}>
                    {roleMap[role] || 'Người dùng'}
                </span>
            ),
            filters: [
                { text: 'Quản trị viên', value: 'Manager' },
                { text: 'Quản lý', value: 'Admin' },
                { text: 'Người dùng', value: 'User' },
            ],
            onFilter: (value, record) => record.role.startsWith(value),
            filterSearch: true,
        },
        {
            title: 'Trạng thái', dataIndex: 'isDeleted', key: 'isDeleted', align: 'center',
            render: (text) => (
                <span className={text ? "text-red-500 font-medium" : "text-green-500 font-medium"}>
                    {text ? "Vô hiệu hóa" : "Kích hoạt"}
                </span>
            ),
            filters: [
                { text: 'Kích hoạt', value: false },
                { text: 'Vô hiệu hóa', value: true },
            ],
            onFilter: (value, record) => record.isDeleted === value,
            filterSearch: true,
        },
        {
            title: 'Thao tác', key: 'actions', align: 'center', width: 150,
            render: (_, record) => {
                const currentUserRole = localStorage.getItem('role');
                const isManager = record.role === 'Manager';
                const isAdmin = currentUserRole === 'Admin';
                const isUser = record.role === 'User';

                const canEdit = (currentUserRole === 'Manager' && !isManager) || (isAdmin && isUser);
                const canActivate = record.isDeleted && canEdit;
                const canDisable = !record.isDeleted && canEdit;

                return (
                    <div className='flex gap-2 justify-center'>
                        <Tooltip title="Cập nhật chức năng" placement='bottom' color={"gold"}>
                            <Button
                                className={canEdit ? "px-3 py-5 border-yellow-500 hover:!border-yellow-500" : "px-3 py-5 !border-gray-400"}
                                disabled={!canEdit}
                                onClick={() => handleOpenModal(record)}>
                                <EditOutlined className={canEdit ? 'text-yellow-500' : 'text-gray-400'} />
                            </Button>
                        </Tooltip>

                        <Popconfirm
                            icon={<QuestionCircleOutlined style={{ color: 'red' }} />}
                            title="Xác nhận kích hoạt người dùng?"
                            okText="Xác nhận"
                            cancelText="Hủy"
                            onConfirm={() => {
                                selectCustomerID.current = record.customerID;
                                handleUpdateCustomerStatus(false);
                            }}>
                            <Tooltip title="Kích hoạt người dùng" placement='bottom' color={"blue"}>
                                <Button
                                    className={canActivate ? "px-3 py-5" : "px-3 py-5 !border-gray-400"}
                                    disabled={!canActivate}>
                                    <CheckOutlined className={canActivate ? 'text-custom1' : 'text-gray-400'} />
                                </Button>
                            </Tooltip>
                        </Popconfirm>

                        <Popconfirm
                            icon={<QuestionCircleOutlined style={{ color: 'red' }} />}
                            title="Xác nhận vô hiệu hóa người dùng?"
                            okText="Xác nhận"
                            cancelText="Hủy"
                            onConfirm={() => {
                                selectCustomerID.current = record.customerID;
                                handleUpdateCustomerStatus(true);
                            }}>
                            <Tooltip title="Vô hiệu hóa người dùng" placement='bottom' color={"red"}>
                                <Button
                                    danger
                                    className={canDisable ? "px-3 py-5" : "px-3 py-5 !border-gray-400"}
                                    disabled={!canDisable}>
                                    <CloseOutlined className={canDisable ? 'text-lg' : 'text-gray-400'} />
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
                    { title: "Quản lý người dùng" }
                ]}
            />
            <div className='bg-white mt-4 p-4 rounded-md shadow-md' style={{ minHeight: 'calc(100vh - 8rem)' }}>

                <Input
                    placeholder="Tìm người dùng..."
                    prefix={<SearchOutlined className='mr-2 ' />}
                    onChange={(e) => setSearchText(e.target.value)}
                    className='mb-4'
                    style={{ width: 400 }}
                />

                <Table
                    columns={columns}
                    dataSource={filteredCustomer}
                    rowKey="customerID"
                    loading={loading}
                    pagination={{ pageSize: 6 }}
                />
            </div>
            <Modal
                open={isModalOpen}
                onCancel={handleCancelModal}
                footer={null}
                width={350}
                style={{ top: 150 }}
            >
                <p className='text-center text-xl text-custom1 font-bold mb-6'>CẬP NHẬT CHỨC NĂNG</p>
                <Form form={form} onFinish={handleSubmit} layout="horizontail" requiredMark={false}>
                    <Form.Item
                        name="role"
                        label="Chức năng"
                        rules={[{ required: true, message: 'Vui lòng chọn chức năng!' }]}
                    >
                        <Select
                            placeholder="Chọn chức năng"
                            className='mb-1 mt-2'
                            options={[
                                { label: 'Quản lý', value: 'Admin' },
                                { label: 'Người dùng', value: 'User' }
                            ]}
                        />
                    </Form.Item>
                    <Form.Item>
                        <Button type='primary' htmlType="submit" className="w-full h-10 mt-2">
                            Cập nhật
                        </Button>
                    </Form.Item>
                </Form>
            </Modal>
        </>
    )
}

export default ManageUser;