import React from "react";
import { Form, Input, Button, App } from 'antd';
import { MailOutlined, PhoneOutlined, EnvironmentOutlined } from '@ant-design/icons';
import slider1 from '../../assets/images/slider1.jpg';
import { SendMailContact } from '../../services/AuthService'

const Contact = () => {
    const { message } = App.useApp();
    const [form] = Form.useForm();
    const onFinish = async (values) => {
        try {
            const response = await SendMailContact(values);
            if (response.status === 200) {
                message.success('Gửi liên hệ thành công! Chúng tôi sẽ phản hồi sớm.');
                form.resetFields();
            } else {
                message.error('Gửi liên hệ thất bại.');
            }
        }
        catch (errors) {
            message.error('Lỗi khi gửi liên hệ.');
            console.log(errors);
        }
    };
    return (
        <>
            <span
                className="w-full h-[450px] block bg-center"
                style={{ backgroundImage: `url(${slider1})` }}
            />
            <div className="max-w-screen-xl mx-auto mt-10">
                <div className="text-center text-4xl font-bold text-custom1">LIÊN HỆ VỚI CHÚNG TÔI</div>
                <p className="text-center text-lg text-gray-600 mt-4">
                    Hãy điền thông tin vào form dưới đây hoặc liên hệ trực tiếp với chúng tôi qua các phương thức dưới đây.
                </p>

                <div className="mt-8 grid grid-cols-2 gap-6">
                    <Form form={form}
                        name="contact"
                        onFinish={onFinish}
                        layout="vertical"
                        initialValues={{ remember: true }}
                        className="p-6 rounded-md shadow border border-gray-300"
                    >
                        <Form.Item
                            label="Họ và tên"
                            name="fullName"
                            rules={[{ required: true, message: 'Vui lòng nhập họ và tên!' }]}
                        >
                            <Input placeholder="Nhập họ và tên của bạn" className="mb-1" />
                        </Form.Item>

                        <Form.Item
                            label="Email"
                            name="email"
                            rules={[
                                { required: true, message: 'Vui lòng nhập email!' },
                                { type: 'email', message: 'Email không hợp lệ!' }
                            ]}
                        >
                            <Input placeholder="Nhập email của bạn" className="mb-1" />
                        </Form.Item>

                        <Form.Item
                            label="Số điện thoại"
                            name="phone"
                            rules={[
                                { required: true, message: 'Vui lòng nhập số điện thoại!' },
                                { pattern: /^[0-9]{10}$/, message: 'Số điện thoại là 10 số' }
                            ]}
                        >
                            <Input maxLength={10} placeholder="Nhập số điện thoại của bạn" className="mb-1" />
                        </Form.Item>

                        <Form.Item
                            label="Nội dung"
                            name="message"
                            rules={[{ required: true, message: 'Vui lòng nhập nội dung!' }]}
                        >
                            <Input.TextArea
                                placeholder="Nhập câu hỏi hoặc yêu cầu của bạn"
                                rows={4}
                                className="mb-1" />
                        </Form.Item>

                        <Form.Item>
                            <Button type="primary" htmlType="submit" className="w-full h-12 mt-4 text-lg">
                                Gửi Liên Hệ
                            </Button>
                        </Form.Item>
                    </Form>
                    <div className="p-6  rounded-md shadow border border-gray-300">
                        <h2 level={2} className="text-2xl text-custom1 mb-4 font-bold">THÔNG TIN LIÊN HỆ</h2>
                        <p className="text-gray-700">
                            <MailOutlined className="mr-2 text-velvet3" /> Email: info@cuahang.com
                        </p>
                        <h2 className="text-gray-700">
                            <PhoneOutlined className="mr-2 text-velvet3" /> Số điện thoại: +84 123 456 789
                        </h2>
                        <p className="text-gray-700">
                            <EnvironmentOutlined className="mr-2 text-velvet3" /> Địa chỉ: 123, Đường ABC, Quận XYZ, TP Hồ Chí Minh
                        </p>

                        <div className="w-full mt-6 border border-gray-300 overflow-hidden">
                            <iframe
                                title="Google Maps Location"
                                src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3924.02330629317!2d105.64124277368369!3d10.419723489708167!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x310a64d83b2792df%3A0x17bca5e601420f5a!2zVHLGsOG7nW5nIMSQ4bqhaSBo4buNYyDEkOG7k25nIFRow6Fw!5e0!3m2!1svi!2s!4v1733123560401!5m2!1svi!2s"
                                width="100%"
                                height="400"
                                allowFullScreen
                                loading="lazy"
                                referrerPolicy="no-referrer-when-downgrade"
                            />
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default Contact;