import React from 'react';
import { Image } from 'antd'
import { PhoneOutlined, FacebookOutlined, MailOutlined, InstagramOutlined, EnvironmentOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';
import logo from '../assets/images/logo.png'

const UserFooter = () => {
    return (
        <footer className="bg-custom1 text-white py-10 overflow-hidden mt-10">
            <div className="flex justify-between max-w-screen-xl mx-auto">
                <div className="flex flex-col items-center">
                    <Image width={110} src={logo} />
                    <h3 className="text-3xl font-bold">BOOK STORE</h3>
                </div>

                <div>
                    <h4 className="text-lg font-bold mb-3">THÔNG TIN CỬA HÀNG</h4>
                    <p className="flex items-center mb-2">
                        <EnvironmentOutlined className="mr-2" />
                        Địa chỉ: Số 123, Lấp Vò, Đồng Tháp
                    </p>
                    <p className="flex items-center mb-2">
                        <PhoneOutlined className="mr-2" />
                        Số điện thoại: 0123456789
                    </p>
                    <p className="flex items-center">
                        <MailOutlined className="mr-2" />
                        Email: info@cuahang.com
                    </p>
                </div>

                <div>
                    <h4 className="text-lg font-bold mb-3">MẠNG XÃ HỘI</h4>
                    <p className="flex items-center mb-2 hover:text-custom4">
                        <FacebookOutlined className="mr-2" />
                        <Link to="https://facebook.com" target="_blank" className="hover:text-custom4">
                            Facebook
                        </Link>
                    </p>
                    <p className="flex items-center hover:text-custom4">
                        <InstagramOutlined className="mr-2" />
                        <Link to="https://instagram.com" target="_blank" className="hover:text-custom4">
                            Instagram
                        </Link>
                    </p>
                </div>

                <div>
                    <h4 className="text-lg font-bold mb-3">DANH MỤC</h4>
                    <p className="mb-2">
                        <Link to="/home" className="hover:text-custom4">
                            Trang chủ
                        </Link>
                    </p>
                    <p className="mb-2">
                        <Link to="/about" className="hover:text-custom4">
                            Về chúng tôi
                        </Link>
                    </p>
                    <p>
                        <Link to="/contact" className="hover:text-custom4">
                            Liên hệ
                        </Link>
                    </p>
                </div>
            </div>
        </footer>
    );
};

export default UserFooter;
