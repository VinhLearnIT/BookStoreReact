import React from 'react';
import { Form, Input, message } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { Link, useNavigate } from 'react-router-dom';
import * as authService from '../Services/AuthService';
const LoginForm = () => {

    const navigate = useNavigate();
    const onFinish = async (values) => {

        try {
            const response = await authService.Login(values);
            if (response.status === 200) {
                var data = response.data;
                message.success('Đăng nhập thành công!');
                localStorage.setItem('accessToken', data.accessToken);
                localStorage.setItem('refreshToken', data.refreshToken);
                sessionStorage.setItem('userID', JSON.stringify(data.customerID));
                navigate("/admin")
            } else if (response.status === 401) {
                message.error(response.data.message);
            }
        } catch (error) {
            message.error("Lỗi không xác định!");
            console.log(error);
        }
    };

    return (
        <div className="flex justify-center items-center h-screen bg-gray-100">
            <div className="p-8 bg-white rounded-lg shadow-lg w-full max-w-md">
                <h2 className="text-3xl font-bold text-center mb-16 mt-5 text-velvet3">ĐĂNG NHẬP</h2>
                <Form onFinish={onFinish} layout="vertical" className='text-base'>
                    <Form.Item
                        name="username"
                        rules={[{ required: true, message: 'Vui lòng nhập tên đăng nhập!' }]}
                    >
                        <Input
                            prefix={<UserOutlined className='mr-3 ' />}
                            className='text-base mb-1 border-velvet3'
                            placeholder="Tên đăng nhập"
                        />
                    </Form.Item>
                    <Form.Item
                        name="password"
                        rules={[{ required: true, message: 'Vui lòng nhập mật khẩu!' }]}
                    >
                        <Input.Password
                            prefix={<LockOutlined className='mr-3' />}
                            className='text-base mb-1 mt-1 border-velvet3'
                            placeholder="Mật khẩu"
                        />
                    </Form.Item>
                    <div className="text-right mt-4 ">
                        <Link to="/auth/forgot-password" className="text-velvet3 hover:text-velvet3 hover:opacity-75">
                            Quên mật khẩu?
                        </Link>
                    </div>
                    <Form.Item>
                        <button
                            type='submit'
                            className="w-full mt-4 h-12 text-base rounded-md bg-velvet3 text-white hover:opacity-85 "
                        >
                            Đăng nhập
                        </button>
                    </Form.Item>

                    <div className="text-center mt-4 text-gray-600">
                        Chưa có tài khoản?{' '}
                        <Link to="/auth/register" className="text-velvet3 hover:text-velvet3 hover:opacity-75 ">
                            Đăng ký ngay
                        </Link>
                    </div>
                </Form>
            </div>
        </div>
    );
};

export default LoginForm;
