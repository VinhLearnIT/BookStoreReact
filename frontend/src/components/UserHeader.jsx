import React, { useState, useContext, useEffect } from 'react';
import { NavLink, Link, useNavigate } from 'react-router-dom';
import { Dropdown, Button } from 'antd';
import {
    InfoCircleOutlined, ShoppingCartOutlined, SearchOutlined, UserOutlined, HomeOutlined, ProductOutlined,
    CommentOutlined, PhoneOutlined, DownOutlined, LoginOutlined, ProfileOutlined, LogoutOutlined, OrderedListOutlined
} from '@ant-design/icons';
import { SearchBooks } from '../services/BookService';
import { GetCountCartByCustomerId } from '../services/ShoppingCartService';
import { CartContext } from '../components/CartContext';
import logo from '../assets/images/logo.png'
const UserHeader = () => {
    const navigate = useNavigate();
    const { cartCount, setCartQuantity } = useContext(CartContext);
    const isAuthenticated = localStorage.getItem('isAuthenticated') === 'true';
    const expireTime = localStorage.getItem('expireTime');
    const currentTime = new Date();
    const isLogin = isAuthenticated && expireTime && new Date(expireTime) > currentTime;
    const [searchResults, setSearchResults] = useState([]);
    const [loading, setLoading] = useState(false);
    const [query, setQuery] = useState("");

    useEffect(() => {
        const fetchCartQuantity = async () => {
            if (isLogin) {
                try {
                    const customerID = localStorage.getItem('customerID');
                    const response = await GetCountCartByCustomerId(customerID);
                    if (response.status === 200) {
                        setCartQuantity(response.data.cartCount);
                    }
                } catch (error) {
                    console.error('Lỗi khi lấy dữ liệu:', error);
                }
            } else {
                let cart = JSON.parse(localStorage.getItem('cart')) || [];
                setCartQuantity(cart.length);
            }
        };
        fetchCartQuantity();
    }, [setCartQuantity, isLogin])
    const handleSearchChange = async (e) => {
        const value = e.target.value;
        setQuery(value);
        try {
            if (value !== "") {
                setLoading(true);
                const response = await SearchBooks(value);
                if (response.status === 200) {
                    setSearchResults(response.data);
                } else {
                    setSearchResults([]);
                }
            } else {
                setSearchResults([]);
            }
        } catch (error) {
            console.error("Có lỗi khi tìm kiếm:", error);
            setSearchResults([]);
        } finally {
            setLoading(false);
        }
    };


    const debounce = (func, delay) => {
        let timer;
        return (...args) => {
            clearTimeout(timer);
            timer = setTimeout(() => func(...args), delay);
        };
    };
    const debouncedSearchChange = debounce(handleSearchChange, 500);

    const items = [
        {
            key: '1',
            label:
                <Button type='text' className='p-0 hover:!bg-transparent hover:!text-custom1 font-semibold'
                    onClick={() => navigate('/account')}
                >
                    <ProfileOutlined className='mr-2' />
                    Thông tin cá nhân
                </Button>
        },
        {
            key: '2',
            label:
                <Button type='text' className='p-0 hover:!bg-transparent hover:!text-custom1 font-semibold'
                    onClick={() => navigate('/order')}
                >
                    <OrderedListOutlined className='mr-2' />
                    Xem đơn hàng
                </Button>
        },
        {
            key: '3',
            label:
                <Button type='text' className='p-0 hover:!bg-transparent hover:!text-custom1 font-semibold'
                    onClick={() => {
                        localStorage.setItem('isAuthenticated', false);
                        localStorage.removeItem('expireTime');
                        navigate('/auth/login');
                    }}
                >
                    <LogoutOutlined className='mr-2' />
                    Đăng xuất
                </Button>
        }
    ];

    return (
        <>
            <header className="bg-custom2 text-white shadow-md py-1 px-8">
                <div className="flex justify-between items-center max-w-screen-xl mx-auto">
                    <div className="font-bold text-white flex flex-col items-center">
                        <img src={logo} alt="logo" className='h-16 w-20' />
                        <span>BOOK STORE</span>
                    </div>
                    <div className="relative w-1/3">
                        <div className="absolute inset-y-0 left-3 flex items-center pointer-events-none">
                            <SearchOutlined className="text-gray-600" />
                        </div>
                        <input
                            type="text"
                            placeholder="Tìm kiếm sản phẩm..."
                            className="w-full pl-10 pr-4 py-2 rounded-md bg-white text-black focus:outline-none"
                            onChange={debouncedSearchChange}
                        />
                        {query && !loading && searchResults.length === 0 && (
                            <div className="absolute left-0 right-0 bg-white shadow-lg mt-2 rounded-md max-h-60 overflow-y-auto z-10 ">
                                <p className="text-gray-500 font-semibold text-center p-2">Không có kết quả mong muốn !</p>
                            </div>
                        )}
                        {searchResults.length > 0 && !loading && (
                            <div className="absolute left-0 right-0 bg-white shadow-lg mt-2 rounded-md max-h-60 overflow-y-auto z-10">
                                {searchResults.map((book) => (
                                    <Link
                                        to={`/book-detail/${book?.bookID}`}
                                        key={book?.bookID}
                                        className="grid grid-cols-[1.5fr_8fr] gap-4 px-4 py-2 hover:bg-gray-100 text-gray-700 hover:text-custom1 "
                                    >
                                        <div className='w-full mr-4 overflow-hidden rounded-md'>
                                            <img
                                                src={`https://localhost:7138/api/images/${book?.imagePath}`}
                                                alt={book?.bookName}
                                                className="object-cover" />
                                        </div>
                                        <div className='font-semibold flex justify-between items-center overflow-hidden'>
                                            <p className='truncate w-2/3'>{book?.bookName}</p>
                                            <p className='text-red-500 text-nowrap text-right'>{book?.price?.toLocaleString()} VNĐ</p>
                                        </div>
                                    </Link>
                                ))}
                            </div>
                        )}
                    </div>

                    <div className="flex items-center space-x-4">
                        <Link to="/shopping-cart" className="border relative border-white px-3 py-2 rounded-md hover:text-custom4 font-semibold">
                            <ShoppingCartOutlined className="mr-2" />
                            Giỏ hàng
                            {cartCount > 0 && (
                                <span className="absolute -top-2 -right-2 bg-red-500 text-white text-xs font-bold rounded-full px-2 py-1">
                                    {cartCount}
                                </span>
                            )}
                        </Link>
                        {!isLogin ? (
                            <Link to="/auth/login" className="border border-white px-3 py-2 rounded-md hover:text-custom4 font-semibold">
                                <LoginOutlined className="mr-2" />
                                Đăng nhập
                            </Link>
                        ) : (
                            <Dropdown menu={{ items }} className='px-3 py-2 border border-white hover:text-custom4 rounded-md font-semibold'>
                                <div className="cursor-pointer">
                                    <UserOutlined className='mr-2' />
                                    Tài khoản
                                    <DownOutlined className='ml-2' />
                                </div>
                            </Dropdown>
                        )}
                    </div>
                </div>
            </header>

            {/* Header Phụ */}
            <nav className="bg-custom1 py-3 shadow-md">
                <div className="flex justify-between max-w-screen-xl mx-auto text-white">
                    <ul className="flex space-x-8 text-lg">
                        <li>
                            <NavLink to="/home"
                                className={({ isActive }) => (isActive ? "text-custom4 hover:text-custom4" : "hover:text-custom4")}>
                                <HomeOutlined className="mr-2" />
                                Trang chủ
                            </NavLink>
                        </li>
                        <li>
                            <NavLink to="/books"
                                className={({ isActive }) => (isActive ? "text-custom4 hover:text-custom4" : "hover:text-custom4")}>
                                <ProductOutlined className="mr-2" />
                                Sản phẩm
                            </NavLink>
                        </li>
                        <li>
                            <NavLink to="/about"
                                className={({ isActive }) => (isActive ? "text-custom4 hover:text-custom4" : "hover:text-custom4")}>
                                <InfoCircleOutlined className="mr-2" />
                                Về chúng tôi
                            </NavLink>
                        </li>
                        <li>
                            <NavLink to="/contact"
                                className={({ isActive }) => (isActive ? "text-custom4 hover:text-custom4" : "hover:text-custom4")}>
                                <CommentOutlined className="mr-2" />
                                Liên hệ
                            </NavLink>
                        </li>
                    </ul>
                    <p className="text-custom4">
                        <PhoneOutlined className="mr-2" />
                        Hotline: 0123456789
                    </p>
                </div>
            </nav>
        </>
    );
};

export default UserHeader;
