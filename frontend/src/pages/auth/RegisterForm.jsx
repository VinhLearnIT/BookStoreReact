import React, { useState, useRef } from 'react';
import { Form, Input, App, Button } from 'antd';
import { MailOutlined, SafetyOutlined, LockOutlined, UserOutlined, UserAddOutlined } from '@ant-design/icons';
import { useNavigate, Link } from 'react-router-dom';
import * as authService from '../../services/AuthService';

const RegisterForm = () => {
    const { message } = App.useApp();
    const [form] = Form.useForm();
    const [countdown, setCountdown] = useState(0);
    const navigate = useNavigate();
    const veryfiCode = useRef("")
    const timerRef = useRef(null);
    const otpResetTimerRef = useRef(null);

    const handleSendOTP = async () => {
        const email = form.getFieldValue('email');
        if (!email) {
            message.error('Vui lòng nhập email trước khi gửi OTP.');
            return;
        }

        if (timerRef.current) {
            clearInterval(timerRef.current);
            timerRef.current = null;
        }
        if (otpResetTimerRef.current) {
            clearTimeout(otpResetTimerRef.current);
            otpResetTimerRef.current = null;
        }

        setCountdown(30);
        timerRef.current = setInterval(() => {
            setCountdown((prev) => {
                if (prev === 1) {
                    clearInterval(timerRef.current);
                    timerRef.current = null;
                    return 0;
                }
                return prev - 1;
            });
        }, 1000);
        message.info('Mã OTP đã được gửi đến email của bạn.');
        try {
            const response = await authService.SendVerificationCode({ email });
            if (response.status === 200) {
                veryfiCode.current = response.data.verificationCode
                otpResetTimerRef.current = setTimeout(() => {
                    veryfiCode.current = "";
                }, 180000);
            } else if (response.status === 404) {
                message.error(response.message);
            }
        } catch (error) {
            message.error("Lỗi không xác định!");
            console.log(error);
        }
    };

    const onFinish = async (values) => {
        if (values.otp !== veryfiCode.current) {
            message.error('Mã OTP không đúng!');
            return;
        }
        var formData = {
            fullName: values.fullName,
            email: values.email,
            username: values.username,
            password: values.password,
            role: "User"
        }
        try {
            const response = await authService.Register(formData);
            if (response.status === 200) {
                message.success(response.data.message)
                navigate("/auth/login");
            } else if (response.status === 400) {
                message.error(response.message);
            }
        } catch (error) {
            message.error("Lỗi không xác định!");
            console.log(error);
        }
    };

    return (
        <div className="p-8 bg-white rounded-lg shadow-lg w-full max-w-lg">
            <h2 className="text-3xl font-bold text-center mb-10 mt-5 text-custom1">ĐĂNG KÍ TÀI KHOẢN</h2>
            <Form form={form} onFinish={onFinish} layout="horizontal" requiredMark={false}
                labelCol={{ span: 8 }} className='custom'>
                <Form.Item
                    name="fullName"
                    label="Họ và tên"
                    rules={[{ required: true, message: 'Vui lòng nhập họ và tên!' }]}>
                    <Input
                        prefix={<UserAddOutlined className='mr-3 ' />}
                        className='mb-1 mt-2 '
                        placeholder="Nhập họ và tên"
                    />
                </Form.Item>

                <Form.Item
                    name="email"
                    label="Email"
                    rules={[
                        { required: true, message: 'Vui lòng nhập email!' },
                        { type: 'email', message: 'Email không hợp lệ!' }
                    ]}>
                    <Input
                        prefix={<MailOutlined className='mr-3 ' />}
                        className='mb-1 mt-2 '
                        placeholder="Nhập email"
                    />
                </Form.Item>

                <Form.Item
                    name="username"
                    label="Tên đăng nhập"
                    rules={[
                        { required: true, message: 'Vui lòng nhập tên đăng nhập!' },
                        { min: 8, message: 'Tên đăng nhập phải có ít nhất 8 ký tự!' }
                    ]}>
                    <Input
                        prefix={<UserOutlined className='mr-3 ' />}
                        className='mb-1 mt-2 '
                        placeholder="Nhập tên đăng nhập"
                    />
                </Form.Item>

                <Form.Item
                    name="password"
                    label="Mật khẩu"
                    rules={[
                        { required: true, message: 'Vui lòng nhập mật khẩu!' },
                        { min: 8, message: 'Mật khẩu phải có ít nhất 8 ký tự!' },
                        { pattern: /^(?=.*[a-zA-Z])/, message: 'Mật khẩu phải chứa ít nhất một chữ cái!' }
                    ]}>
                    <Input.Password
                        prefix={<LockOutlined className='mr-3 ' />}
                        className='mb-1 mt-2 '
                        placeholder="Nhập mật khẩu"
                    />
                </Form.Item>

                <Form.Item
                    name="confirmPassword"
                    label="Xác nhận mật khẩu"
                    dependencies={['password']}
                    rules={[
                        { required: true, message: 'Vui lòng nhập lại mật khẩu!' },
                        ({ getFieldValue }) => ({
                            validator(_, value) {
                                if (!value || getFieldValue('password') === value) {
                                    return Promise.resolve();
                                }
                                return Promise.reject('Mật khẩu không khớp!');
                            },
                        })]}>
                    <Input.Password
                        prefix={<LockOutlined className='mr-3 ' />}
                        className='mb-1 mt-2 '
                        placeholder="Xác nhận lại mật khẩu"
                    />
                </Form.Item>
                <div className='flex items-start justify-end'>
                    <Form.Item
                        name="otp"
                        label="Mã OTP"
                        rules={[
                            { required: true, message: 'Vui lòng nhập mã OTP!' },
                            { pattern: /^[0-9]{4}$/, message: 'OTP phải là 4 chữ số!' }
                        ]}>
                        <Input
                            maxLength={4}
                            prefix={<SafetyOutlined className='mr-3 text-lg' />}
                            className='mb-1 mt-2'
                            placeholder="Nhập mã OTP"
                        />
                    </Form.Item>
                    {countdown > 0 ? (
                        <p className="text-center mt-2 min-w-20 ml-3 py-1 text-custom1">Gửi lại {countdown}s</p>
                    ) : (
                        <Button
                            className='px-2 py-1 w-20 ml-3 mt-2'
                            onClick={handleSendOTP}
                        >
                            {countdown > 0 ? `Gửi lại sau ${countdown}s` : 'Gửi OTP'}
                        </Button>
                    )}

                </div>
                <Button
                    type='primary'
                    htmlType="submit"
                    className="w-full h-12 mt-4 text-lg"
                >
                    Đăng kí
                </Button>
                <div className="text-center mt-4 text-gray-600 ">
                    Bạn đã có tài khoản?{' '}
                    <Link to="/auth/login" className="text-custom1 hover:text-custom2">
                        Đăng nhập ngay
                    </Link>
                </div>

            </Form>
        </div>
    );
};

export default RegisterForm;
