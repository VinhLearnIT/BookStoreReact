import React, { useState, useContext } from "react";
import { Pagination, Button, App } from "antd";
import { Link, useNavigate } from 'react-router-dom';
import { ShoppingCartOutlined } from '@ant-design/icons';
import * as shoppingCartService from '../services/ShoppingCartService'
import refreshToken from '../utils/RefreshToken';
import { CartContext } from '../components/CartContext';

const BookList = ({ book, pageSize = 8, col = 4 }) => {
    const navigate = useNavigate();
    const { addToCart } = useContext(CartContext);
    const { message } = App.useApp();
    const [currentPage, setCurrentPage] = useState(1);
    const totalPages = Math.ceil(book.length / pageSize);
    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    const currentBooks = book.slice(startIndex, endIndex);

    const onChangePage = (page) => {
        setCurrentPage(page);
    };

    const refreshAccessToken = async () => {
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
    };

    const AddToCartForGuest = async (book, quantity = 1) => {
        let cart = JSON.parse(localStorage.getItem('cart')) || [];
        const existingItemIndex = cart.findIndex(item => item.bookID === book.bookID);

        let newQuantity = quantity;
        if (existingItemIndex > -1) {
            newQuantity += cart[existingItemIndex].quantity;
        }

        try {
            const stockCheckResponse = await shoppingCartService.CheckStockQuantity(book.bookID, newQuantity);
            if (stockCheckResponse.status !== 200) {
                message.error(stockCheckResponse.message || "Có lỗi xảy ra khi kiểm tra số lượng!");
                return;
            }

            if (existingItemIndex > -1) {
                cart[existingItemIndex].quantity = newQuantity;
            } else {
                cart.push({
                    cartID: Date.now(),
                    bookID: book.bookID,
                    bookName: book.bookName,
                    imagePath: book.imagePath,
                    quantity: quantity,
                    price: book.price
                });
                addToCart(1);
            }

            localStorage.setItem('cart', JSON.stringify(cart));
            message.success("Thêm sản phẩm vào giỏ hàng tạm thời!");
        } catch (error) {
            message.error("Đã xảy ra lỗi khi kiểm tra số lượng sản phẩm!");
            console.error(error);
        }
    };

    const AddToCartForCustomer = async (book, quantity = 1) => {
        const cartData = {
            customerID: localStorage.getItem('customerID'),
            bookID: book.bookID,
            bookName: book.bookName,
            quantity,
            price: book.price
        };
        try {
            let token = localStorage.getItem('accessToken');
            let response = await shoppingCartService.AddToCart(token, cartData);
            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await shoppingCartService.AddToCart(token, cartData);
            }
            if (response.status === 200) {
                if (response.data.message === "New") {
                    addToCart(1);
                }
                message.success("Thêm sản phẩm vào giỏ hàng thành công!");
            } else {
                message.error(response.message || "Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng.");
            }
        } catch (error) {
            console.log(error);
            message.error("Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng.");
        }
    };
    const handleAddToCart = async (book) => {
        const quantity = 1;
        const isAuthenticated = localStorage.getItem('isAuthenticated') === 'true';
        const expireTime = localStorage.getItem('expireTime');
        const currentTime = new Date();
        if (isAuthenticated && expireTime && new Date(expireTime) > currentTime) {
            await AddToCartForCustomer(book, quantity);
        } else {
            await AddToCartForGuest(book, quantity);
        }
    };

    return (
        <>
            <div className={`grid grid-cols-${col} gap-4`}>
                {currentBooks.map((book) => (
                    <div key={book.bookID} className="p-4 border rounded-md shadow-md bg-white hover:shadow-lg transition-shadow duration-300">
                        <Link to={`/book-detail/${book?.bookID}`} >
                            <img
                                src={`https://localhost:7138/api/images/${book?.imagePath}`}
                                alt={book?.bookName}
                                className="w-full h-auto max-h-72 rounded-lg object-cover"
                            />
                        </Link>
                        <div className="p-4 flex flex-col gap-1 items-center w-full overflow-hidden">
                            <h3 className="text-custom1 text-lg font-bold truncate">{book?.bookName}</h3>

                            <p className="text-sm">{book?.author}</p>
                            <p className="text-red-500 font-bold">{book?.price?.toLocaleString()} VNĐ</p>
                            <Button
                                className="mt-2 h-10"
                                type="primary"
                                icon={<ShoppingCartOutlined className="mr-1" />}
                                onClick={() => handleAddToCart(book)}
                            >
                                Thêm vào giỏ hàng
                            </Button>
                        </div>
                    </div>
                ))}
            </div>

            {totalPages > 1 && (
                <div className="flex justify-center mt-6">
                    <Pagination
                        current={currentPage}
                        total={book.length}
                        pageSize={pageSize}
                        onChange={onChangePage}
                        showSizeChanger={false}
                    />
                </div>
            )}
        </>
    );
};

export default BookList;
