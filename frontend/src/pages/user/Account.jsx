import React, { useState, useCallback, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Form, Input, Button, App } from 'antd';
import * as customerService from '../../services/CustomerService'
import refreshToken from '../../utils/RefreshToken';

const Account = () => {
    const navigate = useNavigate();
    const { message } = App.useApp();
    const [formInfo] = Form.useForm();
    const [formPass] = Form.useForm();
    const [customer, setCustomer] = useState(null);
    const [selectedMenu, setSelectedMenu] = useState('updateInfo');
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

    const getCustomer = useCallback(async () => {
        try {
            const customerID = localStorage.getItem('customerID');
            let token = localStorage.getItem('accessToken');
            let response = await customerService.GetCustomerById(token, customerID);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                token = localStorage.getItem('accessToken');
                response = await customerService.GetCustomerById(token, customerID);
            }
            if (response.status === 200) {
                setCustomer(response.data);
            } else if (response.status === 404) {
                message.error(response.message);
            }
        } catch (error) {
            message.error("Không thể tải dữ liệu!");
            console.log(error);
        }
    }, [message, refreshAccessToken]);

    useEffect(() => {

        getCustomer();
    }, [getCustomer]);

    const handleUpdatePassword = async (values) => {
        try {
            const customerID = localStorage.getItem('customerID');
            let token = localStorage.getItem('accessToken');
            const formData = { ...values, customerID }
            let response = await customerService.UpdatePassword(token, formData);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await customerService.UpdatePassword(token, formData);
            }
            if (response.status === 200) {
                formPass.resetFields();
                message.success(response.data.message)
            } else if (response.status === 404 || response.status === 400) {
                message.error(response.message);
            }
        } catch (error) {
            message.error("Lỗi khi cập nhật mật khẩu!");
            console.log(error);
        }
    };

    const handleUpdateInfo = async (values) => {
        try {
            const isSame = Object.keys(values).every(
                (key) => values[key] === customer[key]
            );

            if (isSame) {
                message.info("Không có thay đổi nào để cập nhật!");
                return;
            }
            const customerID = localStorage.getItem('customerID');
            const formData = { ...values }
            let token = localStorage.getItem('accessToken');
            let response = await customerService.UpdateCustomer(token, customerID, formData);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await customerService.UpdateCustomer(token, customerID, formData);
            }
            if (response.status === 200) {
                message.success('Cập nhật thông tin thành công!');
            } else if (response.status === 404 || response.status === 400) {
                message.error(response.message);
            }
        } catch (error) {
            message.error("Lỗi khi cập nhật thông tin!");
            console.log(error);
        }
    };

    return (
        <div className="flex max-w-screen-xl mx-auto my-20" style={{ minHeight: 'calc(100vh - 20rem)' }}>
            <div className="w-1/4 border border-custom1 p-5 h-fit rounded-md shadow-md">
                <h2 className="text-lg font-bold text-custom1 mb-4 pb-2 text-center border-b border-custom1 border-dashed">
                    QUẢN LÝ TÀI KHOẢN
                </h2>
                <div className="flex flex-col gap-4">
                    <Button
                        type='text'
                        className={`p-5 rounded text-custom1 hover:!bg-gray-200 hover:!text-custom1 
                            font-semibold ${selectedMenu === "updateInfo" ? "bg-gray-200" : ""}`}
                        onClick={() => setSelectedMenu('updateInfo')}
                    >
                        Cập nhật thông tin
                    </Button>
                    <Button
                        type='text'
                        className={`p-5 rounded text-custom1 hover:!bg-gray-200 hover:!text-custom1 
                            font-semibold ${selectedMenu === "changePassword" ? "bg-gray-200" : ""}`}
                        onClick={() => setSelectedMenu('changePassword')}
                    >
                        Cập nhật mật khẩu
                    </Button>
                </div>
            </div>

            {selectedMenu === 'updateInfo' && customer && (
                <div className="flex-1 max-w-[700px] p-5 border border-custom1 rounded-md mx-auto h-fit">
                    <h2 className='text-lg font-bold text-custom1 text-center mb-4 '>CẬP NHẬT THÔNG TIN</h2>
                    <Form form={formInfo} layout="vertical" onFinish={handleUpdateInfo} className='w-2/3 mx-auto'>
                        <Form.Item
                            initialValue={customer?.fullName}
                            name="fullName"
                            label="Họ và tên"
                            rules={[{ required: true, message: 'Vui lòng nhập họ tên!' }]}>
                            <Input className='mb-1' placeholder="Nhập họ và tên" />
                        </Form.Item>

                        <Form.Item
                            initialValue={customer?.phone}
                            label="Số điện thoại"
                            name="phone"
                            rules={[
                                { required: true, message: 'Vui lòng nhập số điện thoại!' },
                                { pattern: /^[0-9]{10}$/, message: 'Số điện thoại là 10 số' }
                            ]}>
                            <Input maxLength={10} placeholder='Nhập số điện thoại' />
                        </Form.Item>
                        <Form.Item
                            initialValue={customer?.cccd}
                            label="CCCD"
                            name="cccd"
                            rules={[
                                { required: true, message: 'Vui lòng nhập CCCD!' },
                                { pattern: /^[0-9]{12}$/, message: 'Căn cước công dân là 10 số' }
                            ]}>
                            <Input maxLength={12} placeholder='Nhập căn cước công dân' />
                        </Form.Item>
                        <Form.Item
                            initialValue={customer?.address}
                            label="Địa chỉ"
                            name="address"
                            rules={[{ required: true, message: 'Vui lòng nhập địa chỉ!' }]}>
                            <Input.TextArea rows={3} placeholder='Nhập địa chỉ' />
                        </Form.Item>
                        <Form.Item className='flex justify-center w-full '>
                            <Button
                                type="primary"
                                htmlType="submit"
                                className='h-10 mt-4'
                            >
                                Cập nhật thông tin
                            </Button>
                        </Form.Item>
                    </Form>
                </div>
            )}

            {selectedMenu === 'changePassword' && (
                <div className="flex-1 max-w-[700px] p-5 border border-custom1 rounded-md mx-auto h-fit">
                    <h2 className='text-lg font-bold text-custom1 text-center mb-4 '>CẬP NHẬT MẬT KHẨU</h2>
                    <Form form={formPass} layout="vertical" onFinish={handleUpdatePassword} className='w-2/3 mx-auto' >
                        <Form.Item
                            name="oldPassword"
                            label="Mật khẩu hiện tại"
                            rules={[{ required: true, message: 'Vui lòng nhập mật khẩu hiện tại!' }]}>
                            <Input.Password className='mb-1' placeholder="Nhập mật khẩu hiện tại" />
                        </Form.Item>
                        <Form.Item name="newPassword" label="Mật khẩu mới"
                            rules={[
                                { required: true, message: 'Vui lòng nhập mật khẩu mới!' },
                                { min: 8, message: 'Mật khẩu phải có ít nhất 8 ký tự!' },
                                { pattern: /^(?=.*[a-zA-Z])/, message: 'Mật khẩu phải chứa ít nhất một chữ cái!' }
                            ]}>
                            <Input.Password className='mb-1' placeholder="Nhập mật khẩu mới" />
                        </Form.Item>
                        <Form.Item
                            name="confirmPassword"
                            dependencies={['newPassword']}
                            label="Xác nhận mật khẩu mới"
                            rules={
                                [{ required: true, message: 'Vui lòng xác nhận mật khẩu mới!' },
                                ({ getFieldValue }) => ({
                                    validator(_, value) {
                                        if (!value || getFieldValue('newPassword') === value) {
                                            return Promise.resolve();
                                        }
                                        return Promise.reject('Mật khẩu không khớp!');
                                    },
                                })
                                ]}>
                            <Input.Password className='mb-1' placeholder="Xác nhận mật khẩu mới" />
                        </Form.Item>
                        <Form.Item className='flex justify-center w-full'>
                            <Button
                                type="primary"
                                htmlType="submit"
                                className='h-10 mt-4'
                            >
                                Cập nhật mật khẩu
                            </Button>
                        </Form.Item>
                    </Form>
                </div>
            )}
        </div>
    );
};

export default Account;
