import React, { useState, useRef } from 'react';
import { Form, Input, message } from 'antd';
import { MailOutlined, SafetyOutlined, LockOutlined } from '@ant-design/icons';
import { Link, useNavigate } from 'react-router-dom';
import * as authService from '../Services/AuthService';
const ForgotPasswordForm = () => {
    const [otpSent, setOtpSent] = useState(false);
    const [countdown, setCountdown] = useState(0);
    const [form] = Form.useForm();
    const data = useRef({
        customerID: "",
        verificationCode: ""
    })
    const navigate = useNavigate();

    const handleSendOTP = async () => {
        const email = form.getFieldValue('email');
        if (!email) {
            return;
        }

        setOtpSent(true);
        setCountdown(30);
        const timer = setInterval(() => {
            setCountdown((prev) => {
                if (prev === 1) {
                    clearInterval(timer);
                    return 0;
                }
                return prev - 1;
            });
        }, 1000);
        message.info('Mã OTP đã được gửi đến email của bạn.');
        try {
            const response = await authService.SendVerificationCode({ email });
            if (response.status === 200) {
                data.current = response.data
                navigate("/auth/login");
            } else if (response.status === 404) {
                message.error(response.data.message);
            }
        } catch (error) {
            message.error("Lỗi không xác định!");
            console.log(error);
        }

    };

    const onFinish = async (values) => {
        if (otpSent) {

            if (values.otp !== data.current.verificationCode) {
                message.error('Mã OTP không đúng!');
                return;
            }
            var formData = {
                customerID: data.current.customerID,
                newPassword: values.newPassword
            }
            try {
                const response = await authService.ForgotPassword(formData);
                if (response.status === 200) {
                    message.success(response.data.message)
                } else if (response.status === 404) {
                    message.error(response.data.message);
                }
            } catch (error) {
                message.error("Lỗi không xác định!");
                console.log(error);
            }
        }
    };

    return (
        <div className="flex justify-center items-center h-screen bg-gray-100">
            <div className="p-8 bg-white rounded-lg shadow-md w-full max-w-md">
                <h2 className="text-3xl font-bold text-center mb-16 mt-5 text-velvet3">QUÊN MẬT KHẨU</h2>
                <Form form={form} onFinish={onFinish} layout="vertical">
                    <Form.Item
                        name="email"
                        rules={[{ required: true, type: 'email', message: 'Vui lòng nhập email!' }]}>
                        <Input
                            prefix={<MailOutlined className='mr-3 ' />}
                            className='text-base mb-1 border-velvet3'
                            placeholder="Email" />
                    </Form.Item>
                    {otpSent && (
                        <>
                            <Form.Item
                                name="otp"
                                rules={[
                                    { required: true, message: 'Vui lòng nhập OTP!' },
                                    { pattern: /^[0-9]{4}$/, message: 'OTP phải là 4 chữ số!' }
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    prefix={<SafetyOutlined className='mr-3' />}
                                    className='text-base mb-1 mt-1 border-velvet3'
                                    placeholder="Nhập OTP gồm 4 số" />
                            </Form.Item>
                            <Form.Item
                                name="newPassword"
                                rules={[{ required: true, message: 'Vui lòng nhập mật khẩu mới!' }]}
                            >
                                <Input.Password
                                    prefix={<LockOutlined className='mr-3' />}
                                    className='text-base mb-1 mt-1 border-velvet3'
                                    placeholder="Mật khẩu mới" />
                            </Form.Item>
                            <Form.Item
                                name="confirmPassword"
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
                                    className='text-base mb-1 mt-1 border-velvet3'
                                    placeholder="Xác nhận mật khẩu" />
                            </Form.Item>
                        </>
                    )}
                    {!otpSent ? (
                        <Form.Item>
                            <button
                                className="w-full mt-2 h-12 text-base rounded-md bg-velvet3 text-white hover:opacity-85 "
                                onClick={handleSendOTP}
                            >
                                Gửi OTP
                            </button>
                        </Form.Item>
                    ) : (
                        <>
                            <Form.Item>
                                <button
                                    type='submit'
                                    className="w-full mt-4 h-12 text-base rounded-md bg-velvet3 text-white hover:opacity-85 "
                                >
                                    Đặt lại mật khẩu
                                </button>
                            </Form.Item>
                            {countdown > 0 ? (
                                <p className="text-center text-velvet2 text-base">Gửi lại mã sau {countdown} giây</p>
                            ) : (
                                <button
                                    className="w-full h-12 text-base rounded-md text-velvet3"
                                    onClick={handleSendOTP}
                                >
                                    Gửi lại OTP
                                </button>
                            )}
                        </>
                    )}
                    <div className="text-center mt-4 ">
                        <Link to="/auth/login" className="text-velvet3 text-base hover:text-velvet3 hover:opacity-75 ">
                            Trở lại đăng nhập
                        </Link>
                    </div>
                </Form>
            </div>
        </div>
    );
};

export default ForgotPasswordForm;
