import React, { useState, useRef } from 'react';
import { Form, Input, App, Button } from 'antd';
import { MailOutlined, SafetyOutlined, LockOutlined } from '@ant-design/icons';
import { Link, useNavigate } from 'react-router-dom';
import * as authService from '../../services/AuthService';

const ForgotPasswordForm = () => {
    const { message } = App.useApp();
    const [otpSent, setOtpSent] = useState(false);
    const [countdown, setCountdown] = useState(0);
    const [form] = Form.useForm();
    const navigate = useNavigate();
    const veryfiCode = useRef("")
    const timerRef = useRef(null);
    const otpResetTimerRef = useRef(null);

    const handleSendOTP = async () => {
        const email = form.getFieldValue('email');
        if (!email) {
            message.error('Vui lòng nhập email trước khi gửi OTP!');
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

        setOtpSent(true);
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
                message.error(response.data.message);
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
            email: values.email,
            newPassword: values.newPassword
        }
        try {
            const response = await authService.ForgotPassword(formData);
            if (response.status === 200) {
                message.success(response.data.message)
                navigate("/auth/login");
            } else if (response.data.status === 404) {
                message.error(response.data.message);
            }
        } catch (error) {
            message.error("Lỗi không xác định!");
            console.log(error);
        }

    };

    return (
        <div className="p-8 bg-white rounded-lg shadow-md w-full max-w-md">
            <h2 className="text-3xl font-bold text-center mb-10 mt-5 text-custom1">QUÊN MẬT KHẨU</h2>
            <Form form={form} onFinish={onFinish} layout="vertical" requiredMark={false}>
                <Form.Item
                    name="email"
                    label="Email"
                    rules={[
                        { required: true, message: 'Vui lòng nhập email!' },
                        { type: 'email', message: 'Email không hợp lệ!' },
                    ]}>
                    <Input
                        prefix={<MailOutlined className='mr-3 ' />}
                        className='mb-1'
                        placeholder="Nhập email" />
                </Form.Item>
                {otpSent && (
                    <>

                        <Form.Item
                            name="newPassword"
                            label="Mật khẩu mới"
                            rules={[
                                { required: true, message: 'Vui lòng nhập mật khẩu mới!' },
                                { min: 8, message: 'Mật khẩu phải có ít nhất 8 ký tự!' },
                                { pattern: /^(?=.*[a-zA-Z])/, message: 'Mật khẩu phải chứa ít nhất một chữ cái!' },
                                { pattern: /^\S*$/, message: 'Mật khẩu không được chứa khoảng trắng!' }
                            ]}
                        >
                            <Input.Password
                                prefix={<LockOutlined className='mr-3' />}
                                className='mb-1'
                                placeholder="Mật khẩu mới" />
                        </Form.Item>
                        <Form.Item
                            name="confirmPassword"
                            label="Xác nhận mật khẩu"
                            dependencies={['newPassword']}
                            rules={[
                                { required: true, message: 'Vui lòng xác nhận mật khẩu!' },
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
                            <Input.Password
                                prefix={<LockOutlined className='mr-3' />}
                                className='mb-1'
                                placeholder="Xác nhận mật khẩu" />
                        </Form.Item>
                        <div className='flex items-center justify-between'>
                            <Form.Item
                                name="otp"
                                label="Mã OTP"
                                className='flex-1'
                                rules={[
                                    { required: true, message: 'Vui lòng nhập OTP!' },
                                    { pattern: /^[0-9]{4}$/, message: 'OTP phải là 4 chữ số!' }
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    prefix={<SafetyOutlined className='mr-3 text-lg' />}
                                    className='mb-1'
                                    placeholder="Nhập OTP gồm 4 số" />
                            </Form.Item>
                            {countdown > 0 ? (
                                <p className="text-center mt-1 min-w-20 ml-3 py-1 text-custom1">Gửi lại sau {countdown}s</p>
                            ) : (
                                <Button
                                    className="px-2 w-24 ml-3 mt-1"
                                    onClick={handleSendOTP}
                                >
                                    Gửi lại OTP
                                </Button>
                            )}
                        </div>
                    </>
                )}
                {!otpSent ? (
                    <Button
                        type='primary'
                        className="w-full h-12 mt-4 text-lg"
                        onClick={handleSendOTP}
                    >
                        Gửi OTP
                    </Button>
                ) : (
                    <>

                        <Button
                            type='primary'
                            htmlType="submit"
                            className="w-full h-12 mt-4 text-lg"
                        >
                            Đặt lại mật khẩu
                        </Button>

                    </>
                )}
                <div className="text-center mt-4 ">
                    <Link to="/auth/login" className="text-custom1 hover:text-custom2 ">
                        Trở lại đăng nhập
                    </Link>
                </div>
            </Form>
        </div>
    );
};

export default ForgotPasswordForm;
