import React, { useState, useEffect, useCallback } from 'react';
import { App, Carousel } from 'antd';
import * as bookService from '../../services/BookService';
import BookList from '../../components/BooksList';

import slider1 from '../../assets/images/slider1.jpg';
import slider2 from '../../assets/images/slider2.jpg';
import slider3 from '../../assets/images/slider3.jpg';
import slider4 from '../../assets/images/slider4.png';

const Home = () => {
    const { message } = App.useApp();
    const [newBooks, setNewBooks] = useState([]);
    const [sellBooks, setSellBooks] = useState([]);
    const images = [slider1, slider2, slider3];

    const getTopBooks = useCallback(async () => {
        try {
            const response = await bookService.GetTopBooks();
            setNewBooks(response.data.topNew);
            setSellBooks(response.data.topBestSell);
        } catch (error) {
            message.error("Không thể tải dữ liệu sách!");
        }
    }, [message]);

    useEffect(() => {
        getTopBooks();
    }, [getTopBooks]);

    return (
        <>
            <Carousel autoplay autoplaySpeed={5000}>
                {images.map((image, index) => (
                    <div key={index} className="relative">
                        <img src={image} alt={`Slider ${index + 1}`} className="w-full h-[450px] object-cover" />
                        <div className="absolute inset-0 bg-black bg-opacity-40 flex items-center justify-center">
                            <div className="text-center text-white px-6">
                                <h2 className="text-xl md:text-3xl font-bold italic ">
                                    {index === 0 && "Một cuốn sách hay là một người bạn không bao giờ phản bội."}
                                    {index === 1 && "Đọc sách là cách ta du hành khắp thế giới mà không cần bước chân ra khỏi nhà."}
                                    {index === 2 && "Người biết đọc nhưng không đọc chẳng hơn gì kẻ không biết đọc."}
                                </h2>
                                <p className="mt-2 text-sm md:text-lg font-light">
                                    {index === 0 && "— Voltaire —"}
                                    {index === 1 && "— Anonymous —"}
                                    {index === 2 && "— Mark Twain —"}
                                </p>
                            </div>
                        </div>
                    </div>
                ))}
            </Carousel>
            <div className='max-w-screen-xl mx-auto mt-14'>
                <h2 className="bg-custom1 text-white py-3 px-4 text-center text-xl rounded-md">
                    SÁCH MỚI NHẤT
                </h2>

                <div className="grid grid-divs-1 md:grid-divs-2 lg:grid-divs-3 gap-6 mt-8">
                    {newBooks.length > 0 ? (
                        <BookList book={newBooks} />
                    ) : (
                        <p className='text-center text-lg text-green-600'>Đang tải dữ liệu...</p>
                    )}
                </div>
            </div>


            <span className='h-96 w-full block mt-20'
                style={{ backgroundImage: `url(${slider4})` }}
            />

            <article className="my-20 max-w-screen-xl mx-auto flex justify-between">
                <div className='rounded-md shadow overflow-hidden'>
                    <img
                        src={slider3}
                        alt="Example"
                        className="w-full h-80 object-cover" />
                </div>
                <div className="w-1/2">
                    <h2 className="text-2xl font-bold text-center text-custom1 mb-4">TIỀM NĂNG CỦA SÁCH</h2>
                    <p className="text-gray-700 text-justify">
                        Ai cũng biết, sách chứa đựng rất nhiều kiến thức về cuộc sống, giúp chúng ta suy nghĩ sâu sắc hơn,
                        trưởng thành hơn và tích lũy vốn từ vựng phong phú. Vậy tại sao chúng ta không thể duy trì thói quen
                        đọc sách hàng ngày hoặc bất cứ khi nào có thể như một cách tận hưởng cuộc sống, nó giống như một
                        trải nghiệm phong phú đủ mọi cung bậc trên từng trang sách. Nhiều ý kiến cho rằng, đọc sách là
                        phương pháp tự học hiệu quả và thiết thực nhất mà ai cũng có thể làm được. Rèn luyện thói quen đọc
                        sách sẽ mang lại những lợi ích vô cùng to lớn; là một thói quen tốt giúp não bộ của chúng ta luôn
                        khỏe mạnh và linh hoạt. Đọc sách mang lại sự thư thái và sảng khoái, là một nguồn thưởng thức tuyệt vời,
                        chỉ cho chúng ta mọi con đường đi cùng với kiến thức tuyệt vời, nó giúp chúng ta trở nên tốt hơn,
                        thành công hơn trong cuộc sống này.
                    </p>
                </div>
            </article>
            <div className='max-w-screen-xl mx-auto'>
                <h2 className="bg-custom1 text-white py-3 px-4 text-center text-xl rounded-md">
                    SÁCH PHỔ BIẾN
                </h2>

                <div className="grid grid-divs-1 md:grid-divs-2 lg:grid-divs-3 gap-6 mt-8">
                    {sellBooks.length > 0 ? (
                        <BookList book={sellBooks} pageSize={4} />
                    ) : (
                        <p>Đang tải dữ liệu...</p>
                    )}
                </div>
            </div>

            <span
                className="w-full h-96 block bg-center mt-20"
                style={{ backgroundImage: `url(${slider2})` }}
            />

            <article className="my-20 max-w-screen-xl mx-auto flex justify-between items-center">
                <div className="w-1/2">
                    <h2 className="text-2xl font-bold text-center text-custom1 mb-4">THÓI QUEN ĐỌC SÁCH</h2>
                    <p className="text-gray-700 text-justify">
                        Thói quen đọc sách giúp kích thích các dây thần kinh não bộ, giảm chứng mất trí nhớ và Alzheimer,
                        giữ cho não bộ của bạn hoạt động và tham gia ngăn không cho bị mất năng lượng, tránh lão hóa.
                        Khi đọc sách, bạn phải suy nghĩ và ghi nhớ nên làm tăng khả năng liên kết của các noron thần kinh.
                        Nếu thực hiện việc đọc sách nhiều luền sẽ khiến cho bạn trở nên thông hơn.
                    </p>
                </div>
                <div className='rounded-md shadow overflow-hidden'>
                    <img
                        src={slider3}
                        alt="Example"
                        className="w-full h-80 object-cover" />
                </div>
            </article>

        </>
    );
};

export default Home;
