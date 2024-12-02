import React, { useState } from 'react';
import { Dropdown, Avatar, Button, App, Modal } from 'antd';
import {
    UserOutlined, LockOutlined, DashboardOutlined, BookOutlined, AppstoreOutlined, ShoppingCartOutlined, TeamOutlined
} from '@ant-design/icons';
import { Link, Outlet, useNavigate } from 'react-router-dom';

const AdminLayout = () => {
    const navigate = useNavigate();
    const { message } = App.useApp();
    const [open, setOpen] = useState(false);
    const items = [
        {
            key: '1',
            label:
                <Button
                    icon={<LockOutlined className='text-lg' />}
                    onClick={() => {
                        message.info("Đổi thành công");
                    }}
                >
                    Đổi mật khẩu
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
                            onClick={() => setOpen(true)}
                        >
                            <LockOutlined className='text-lg mr-4' />
                            Đăng xuất
                        </button>
                        <Modal
                            title="Xác nhận"
                            open={open}
                            width={350}
                            onOk={() => {
                                sessionStorage.removeItem('isAuthenticated');
                                sessionStorage.removeItem('userID');
                                navigate("/auth/login");
                            }}
                            onCancel={() => setOpen(false)}
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
        </div >
    );
};

export default AdminLayout;
