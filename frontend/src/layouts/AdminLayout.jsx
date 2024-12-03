import React, { useState } from 'react';
import { Dropdown, Avatar, Button, App, Modal, Form, Input } from 'antd';
import {
    UserOutlined, LockOutlined, DashboardOutlined, BookOutlined, AppstoreOutlined, ShoppingCartOutlined, TeamOutlined
} from '@ant-design/icons';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import * as authService from '../services/AuthService';

const AdminLayout = () => {
    const navigate = useNavigate();
    const { message } = App.useApp();
    const [openLogoutModal, setOpenLogoutModal] = useState(false);
    const [openPasswordModal, setOpenPasswordModal] = useState(false);
    const [form] = Form.useForm();

    const handlePasswordChange = async (values) => {

        try {
            const customerID = localStorage.getItem('userID');
            const formData = { ...values, customerID }
            const response = await authService.UpdatePassword(formData);
            if (response.status === 200) {
                message.success(response.data.message)
                setOpenPasswordModal(false);
            } else if (response.status === 404 || response.status === 400) {
                message.error(response.message);
                console.log(response);
            }
        } catch (error) {
            message.error("Lỗi không xác định!");
            console.log(error);
        }


    };
    const items = [
        {
            key: '1',
            label:
                <Button
                    icon={<LockOutlined className='text-lg' />}
                    onClick={() => {
                        setOpenPasswordModal(true);
                        form.resetFields();
                    }}
                >
                    Cập nhật mật khẩu
                </Button>
            ,
        }
    ];

    return (
        <div className="min-h-screen relative bg-gray-100">
            <div className="w-72 bg-custom1 text-white fixed h-full z-30">
                <div className="px-4 h-full flex flex-col gap-6">
                    <div className="text-lg h-16 text-center mt-10">
                        LOGO
                    </div>
                    <div className="text-lg ">
                        <Link
                            to="/admin/dash-board"
                            className="flex items-center hover:bg-blue-100 hover:text-custom1 p-2 pl-8 font-medium rounded">

                            <DashboardOutlined className="mr-4" />
                            Trang chủ
                        </Link>
                    </div>
                    <div className="text-lg ">
                        <Link
                            to="/admin/manage-books"
                            className="flex items-center hover:bg-blue-100 hover:text-custom1 p-2 pl-8 font-medium rounded">

                            <BookOutlined className="mr-4" />
                            Quản lý sách
                        </Link>
                    </div>
                    <div className="text-lg">
                        <Link
                            to="/admin/manage-category"
                            className="flex items-center hover:bg-blue-100 hover:text-custom1 p-2 pl-8 font-medium rounded">

                            <AppstoreOutlined className="mr-4" />
                            Quản lý thể loại
                        </Link>
                    </div>
                    <div className="text-lg">
                        <Link
                            to="/admin/manage-order"
                            className="flex items-center hover:bg-blue-100 hover:text-custom1 p-2 pl-8 font-medium rounded">
                            <ShoppingCartOutlined className="mr-4 " />
                            Quản lý đơn hàng
                        </Link>
                    </div>
                    <div className="text-lg ">
                        <Link
                            to="/admin/manage-customer"
                            className="flex items-center hover:bg-blue-100 hover:text-custom1 p-2 pl-8 font-medium rounded">

                            <TeamOutlined className="mr-4" />
                            Quản lý người dùng
                        </Link>
                    </div>
                    <div className="mt-auto mb-10">
                        <button
                            className='flex items-center hover:bg-blue-100 hover:text-custom1 w-full
                                       text-lg p-2 pl-8 font-medium cursor-pointer rounded'
                            onClick={() => setOpenLogoutModal(true)}
                        >
                            <LockOutlined className='text-lg mr-4' />
                            Đăng xuất
                        </button>
                        <Modal
                            title="Xác nhận"
                            open={openLogoutModal}
                            width={350}
                            onOk={() => {
                                localStorage.setItem('isAuthenticated', false);
                                navigate("/auth/login");
                            }}
                            cancelText="Không"
                            okText="Xác nhận"
                            onCancel={() => setOpenLogoutModal(false)}
                        >
                            Bạn có chắc muốn đăng xuất?
                        </Modal>
                    </div>

                </div>
            </div>

            <div className="h-16 bg-white fixed top-0 left-72 right-0 z-10 shadow flex justify-between items-center px-7">
                <div className="text-2xl font-bold text-custom1">QUẢN LÝ CỬA HÀNG BÁN SÁCH</div>
                <Dropdown menu={{ items }} placement="bottom" className='mr-14'>
                    <Avatar size="large" icon={<UserOutlined />} className="cursor-pointer hover:opacity-80" />
                </Dropdown>
            </div>

            <div className='pt-20 ml-80 mr-6'>
                <Outlet />
            </div>
            <Modal
                footer={null}
                width={400}
                open={openPasswordModal}
                onCancel={() => setOpenPasswordModal(false)}
            >
                <p className='text-center text-xl text-custom1 font-bold mb-4'> CẬP NHẬT MẬT KHẨU</p>
                <Form form={form} onFinish={handlePasswordChange} layout="vertical" requiredMark={false}>
                    <Form.Item
                        label="Mật khẩu cũ"
                        name="oldPassword"
                        rules={[{ required: true, message: 'Vui lòng nhập mật khẩu cũ!' }]}
                    >
                        <Input.Password className='mb-1' placeholder="Nhập mật khẩu cũ" />
                    </Form.Item>
                    <Form.Item
                        label="Mật khẩu mới"
                        name="newPassword"
                        rules={[
                            { required: true, message: 'Vui lòng nhập mật khẩu mới!' },
                            { min: 8, message: 'Mật khẩu phải có ít nhất 8 ký tự!' },
                            { pattern: /^(?=.*[a-zA-Z])/, message: 'Mật khẩu phải chứa ít nhất một chữ cái!' }
                        ]}
                    >
                        <Input.Password className='mb-1' placeholder="Nhập mật khẩu mới" />
                    </Form.Item>
                    <Form.Item
                        label="Xác nhận mật khẩu"
                        name="confirmPassword"
                        dependencies={['newPassword']}
                        rules={[
                            { required: true, message: 'Vui lòng xác nhận mật khẩu mới!' },
                            ({ getFieldValue }) => ({
                                validator(_, value) {
                                    if (!value || getFieldValue('newPassword') === value) {
                                        return Promise.resolve();
                                    }
                                    return Promise.reject(new Error('Mật khẩu xác nhận không khớp!'));
                                },
                            }),
                        ]}
                    >
                        <Input.Password className='mb-1' placeholder="Nhập lại mật khẩu mới" />
                    </Form.Item>
                    <Form.Item>
                        <Button type='primary' htmlType="submit" className="w-full h-10 mt-2" >
                            Cập nhật
                        </Button>
                    </Form.Item>
                </Form>
            </Modal>
        </div >
    );
};

export default AdminLayout;
