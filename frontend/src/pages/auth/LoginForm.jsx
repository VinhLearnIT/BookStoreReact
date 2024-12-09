import React from 'react';
import { Form, Input, App, Button } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { Link, useNavigate } from 'react-router-dom';
import * as authService from '../../services/AuthService';
const LoginForm = () => {
    const { message } = App.useApp();
    const navigate = useNavigate();

    const onFinish = async (values) => {
        try {
            const response = await authService.Login(values);
            if (response.status === 200) {
                var data = response.data;
                message.success('Đăng nhập thành công!');
                localStorage.setItem('isAuthenticated', true);
                localStorage.setItem('customerID', JSON.stringify(data.customerID));
                localStorage.setItem('role', data.role);
                localStorage.setItem('accessToken', data.accessToken);
                localStorage.setItem('refreshToken', data.refreshToken);
                const expireTime = new Date();
                expireTime.setDate(expireTime.getDate() + 1);
                localStorage.setItem('expireTime', expireTime);
                if (data.role !== 'User') {
                    navigate("/admin");
                    return;
                }
                navigate('/');
            } else if (response.status === 400) {
                message.error(response.message);
            }
        } catch (error) {
            message.error("Lỗi không xác định!");
            console.log(error);
        }
    };

    return (
        <div className="p-8 bg-white rounded-lg shadow-lg w-full max-w-md">
            <h2 className="text-3xl font-bold text-center mb-10 mt-5 text-custom1">ĐĂNG NHẬP</h2>
            <Form onFinish={onFinish} layout="vertical" requiredMark={false}>
                <Form.Item
                    label="Tên đăng nhập"
                    name="username"
                    rules={[{ required: true, message: 'Vui lòng nhập tên đăng nhập!' }]}
                >
                    <Input
                        prefix={<UserOutlined className='mr-3 ' />}
                        className='mb-1'
                        placeholder="Tên đăng nhập"
                    />
                </Form.Item>
                <Form.Item
                    label="Mật khẩu"
                    name="password"
                    rules={[{ required: true, message: 'Vui lòng nhập mật khẩu!' }]}
                >
                    <Input.Password
                        prefix={<LockOutlined className='mr-3' />}
                        className='mb-1'
                        placeholder="Mật khẩu"
                    />
                </Form.Item>
                <div className="text-right mt-4 ">
                    <Link to="/auth/forgot-password" className="text-custom1 hover:text-custom2">
                        Quên mật khẩu?
                    </Link>
                </div>
                <Form.Item>
                    <Button
                        type='primary'
                        htmlType="submit"
                        className="w-full h-12 mt-4 text-lg"
                    >
                        Đăng nhập
                    </Button>
                </Form.Item>

                <div className="text-center mt-4 text-gray-600">
                    Bạn chưa có tài khoản?{' '}
                    <Link to="/auth/register" className="text-custom1 hover:text-custom2">
                        Đăng ký ngay
                    </Link>
                </div>
            </Form>
        </div>
    );
};

export default LoginForm;
