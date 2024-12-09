import React, { useState, useCallback, useEffect, useContext } from "react";
import { Image, Button, InputNumber, Form, App } from "antd";
import { useParams, useNavigate } from "react-router-dom";
import * as bookService from '../../services/BookService';
import * as shoppingCartService from '../../services/ShoppingCartService';
import refreshToken from "../../utils/RefreshToken";
import BookList from '../../components/BooksList';
import { CartContext } from '../../components/CartContext';

const BookDetail = () => {
    const navigate = useNavigate();
    const { addToCart } = useContext(CartContext);
    const { message } = App.useApp();
    const { id } = useParams();
    const [bookDetail, setDetailBooks] = useState([]);
    const [booksRelate, setBooksRelate] = useState([]);

    const getDetailPage = useCallback(async () => {
        try {
            const responseDetail = await bookService.GetBookById(id);
            const responseRelate = await bookService.GetRelatedBooks(id);
            setDetailBooks(responseDetail.data);
            setBooksRelate(responseRelate.data);
        } catch (error) {
            message.error("Không thể tải dữ liệu!");
        }
    }, [message, id]);

    useEffect(() => {
        getDetailPage();
        window.scrollTo({
            top: 0,
            behavior: "smooth",
        });
    }, [getDetailPage]);


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
        console.log(cartData);

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
    const handleAddToCart = async (values) => {
        const quantity = values.quantity;
        const isAuthenticated = localStorage.getItem('isAuthenticated') === 'true';
        const expireTime = localStorage.getItem('expireTime');
        const currentTime = new Date();

        if (isAuthenticated && expireTime && new Date(expireTime) > currentTime) {
            await AddToCartForCustomer(bookDetail, quantity);
        } else {
            await AddToCartForGuest(bookDetail, quantity);
        }
    };

    return (
        <>

            <h2
                className="text-custom1 text-4xl font-bold border-b border-gray-500 my-12 border-dashed
                text-center w-fit mx-auto pb-2"
            >
                CHI TIẾT SẢN PHẨM
            </h2>

            <div className="flex mt-10 justify-center max-w-screen-xl mx-auto">
                <div className="mr-24 border border-gray-500 rounded-md overflow-hidden">
                    <Image
                        src={bookDetail.imagePath ? `https://localhost:7138/api/images/${bookDetail.imagePath}` : ""}
                        alt={bookDetail.bookName}
                        width={400}
                        height={400}
                        className="rounded-md"
                    />
                </div>

                <div className=" flex flex-col justify-start gap-4">
                    <h1 className="text-3xl font-bold text-custom1">{bookDetail.bookName}</h1>
                    <p className="text-gray-600 mt-2">Tác giả: {bookDetail.author}</p>
                    <p className="text-gray-600 mt-1">Nhà xuất bản: {bookDetail.publisher}</p>
                    <p className="text-red-500 text-lg mt-2 font-bold">
                        Giá: {bookDetail.price?.toLocaleString()} VND
                    </p>

                    <Form onFinish={handleAddToCart} className="mt-4">
                        <Form.Item
                            name="quantity"
                            initialValue={1}
                            rules={[
                                { required: true, message: "Vui lòng nhập số lượng" },
                                {
                                    type: "number", min: 1, max: bookDetail.stockQuantity,
                                    message: `Số lượng tối đa là ${bookDetail.stockQuantity}`
                                },
                            ]}
                        >
                            <InputNumber min={1} max={bookDetail.stockQuantity} className="w-52 mb-1" />
                        </Form.Item>

                        <Form.Item>
                            <Button type="primary" htmlType="submit" className="h-10 w-52 mt-4">
                                Thêm vào giỏ hàng
                            </Button>
                        </Form.Item>
                    </Form>
                </div>
            </div>

            <div className="mt-10 max-w-screen-lg mx-auto ">
                <h2 className="text-3xl font-bold text-custom1 mb-6">Giới thiệu về sách</h2>
                <p className="text-gray-700 text-justify">{bookDetail.description}
                    Thế giới biến đổi không ngừng, lối tư duy mở đóng vai trò vô cùng quan trọng trong việc giúp con người
                    thích nghi, học hỏi và phát triển đặc biệt là trong công việc, học tập.
                    <br /> <br />
                    Vậy tư duy mở là gì? Làm thế nào để rèn luyện và phát triển tư duy mở? Cuốn sách
                    “ỨNG DỤNG TƯ DUY MỞ TRONG CÔNG VIỆC” sẽ giải đáp hết những băn khoăn trên:
                    <br /> <br />
                    Tư duy mở hay còn gọi là tư duy linh hoạt, là khả năng đón nhận, xem xét và chấp nhận các quan điểm,
                    ý tưởng mới một cách cởi mở, không bị ràng buộc bởi các định kiến hay suy nghĩ cố hữu. Đây là một
                    trong những kỹ năng quan trọng nhất của thế kỷ 21, giúp con người thích ứng với sự biến động
                    không ngừng của thế giới.
                    <br /> <br />
                    Trong thế giới hiện đại, tư duy mở là một tố chất cần thiết để thích nghi và phát triển công việc.
                    Các doanh nghiệp, tổ chức có văn hóa mở thường phát triển nhanh hơn và sáng tạo hơn so với những
                    nơi cố hữu, ì trệ về tư duy.
                    <br /> <br />
                    Tư duy mở giúp chúng ta mở rộng tầm nhìn, tiếp thu kiến thức mới, sáng tạo đột phá và giải quyết
                    vấn đề hiệu quả, qua đó đạt được thành công trong sự nghiệp và hạnh phúc trong cuộc sống. Tư duy
                    mở mang lại nhiều lợi ích to lớn cho con người trong cả cuộc sống cá nhân và công việc.
                </p>
            </div>

            <div className="mt-10 max-w-screen-lg mx-auto ">
                <h2 className="text-3xl font-bold text-custom1 mb-6">Sách liên quan</h2>

                {booksRelate.length > 0 ? (
                    <BookList book={booksRelate} />
                ) : (
                    <p>Đang tải dữ liệu...</p>
                )}
            </div>
        </>
    );
};

export default BookDetail;
