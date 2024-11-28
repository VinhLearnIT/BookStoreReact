import React from 'react';
import { Form, Input, Button, message } from 'antd';
import { UserOutlined, MailOutlined, LockOutlined } from '@ant-design/icons';

const RegisterForm = () => {
    const onFinish = (values) => {
        console.log('Register Info:', values);
        message.success('Đăng ký thành công!');
    };

    return (
        <div className="flex justify-center items-center h-screen bg-gray-100">
            <div className="p-8 bg-white rounded-lg shadow-md w-full max-w-md">
                <h2 className="text-2xl font-bold text-center mb-6">Đăng Ký</h2>
                <Form onFinish={onFinish} layout="vertical">
                    <Form.Item name="username" rules={[{ required: true, message: 'Vui lòng nhập tên đăng nhập!' }]}>
                        <Input prefix={<UserOutlined className='mr-3' />} className='text-lg mb-1' placeholder="Tên đăng nhập" />
                    </Form.Item>
                    <Form.Item name="email" rules={[{ required: true, type: 'email', message: 'Vui lòng nhập email hợp lệ!' }]}>
                        <Input prefix={<MailOutlined className='mr-3' />} className='text-lg mt-1 mb-1' placeholder="Email" />
                    </Form.Item>
                    <Form.Item name="password" rules={[{ required: true, message: 'Vui lòng nhập mật khẩu!' }]}>
                        <Input.Password prefix={<LockOutlined className='mr-3' />} className='text-lg mt-1 mb-1' placeholder="Mật khẩu" />
                    </Form.Item>
                    <Form.Item name="confirmPassword" dependencies={['password']} rules={[
                        { required: true, message: 'Vui lòng xác nhận mật khẩu!' },
                        ({ getFieldValue }) => ({
                            validator(_, value) {
                                if (!value || getFieldValue('password') === value) return Promise.resolve();
                                return Promise.reject('Mật khẩu không khớp!');
                            },
                        }),
                    ]}>
                        <Input.Password prefix={<LockOutlined className='mr-3' />} className='text-lg mt-1 mb-1' placeholder="Xác nhận mật khẩu" />
                    </Form.Item>
                    <Form.Item>
                        <Button type="primary" htmlType="submit" className="w-full mt-2 h-10 text-lg">Đăng ký</Button>
                    </Form.Item>
                </Form>
            </div>
        </div>
    );
};

export default RegisterForm;
