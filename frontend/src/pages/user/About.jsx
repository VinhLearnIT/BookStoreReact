import React from "react";
import suMenh from '../../assets/images/sumenh.jpg';
import nhaSach1 from '../../assets/images/nhasach1.jpg';
import nhaSach2 from '../../assets/images/nhasach2.jpg';
import nhaSach3 from '../../assets/images/nhasach3.jpg';
import camKet from '../../assets/images/camket.jpg';
import slider2 from '../../assets/images/slider2.jpg';

const About = () => {
    return (
        <>
            <span
                className="w-full h-[450px] block bg-center"
                style={{ backgroundImage: `url(${slider2})` }}
            />
            <h3 className="text-center text-4xl font-bold text-custom1 my-10">
                VỀ CHÚNG TÔI
            </h3>
            <hr className="w-3/5 border-2 border-dashed border-custom1 mx-auto my-4" />

            <article className="p-6 mb-8">
                <div className="flex flex-col md:flex-row items-center justify-between mx-auto max-w-screen-xl">
                    <div className="w-3/5">
                        <h2 className="text-3xl font-bold text-custom1 mb-4">
                            SỨ MẠNG THÀNH CÔNG CỦA CỬA HÀNG
                        </h2>
                        <p className="text-gray-700 leading-7 text-justify">
                            Sứ mạng thành công của cửa hàng bán sách là mang tri thức đến mọi tầng lớp, nuôi dưỡng
                            tình yêu đọc sách và khơi nguồn cảm hứng học hỏi không ngừng. Cửa hàng cam kết cung cấp
                            đa dạng các thể loại sách, đáp ứng nhu cầu phong phú của khách hàng, từ giáo dục, giải trí
                            đến nghiên cứu chuyên sâu.
                            <br /><br />
                            Bên cạnh đó, cửa hàng tạo ra không gian thân thiện, khuyến khích giao lưu, chia sẻ và
                            xây dựng cộng đồng yêu sách, góp phần thúc đẩy văn hóa đọc và phát triển tri thức xã hội bền vững.
                        </p>
                    </div>
                    <div className="border border-gray-300 rounded-md overflow-hidden shadow">
                        <img src={suMenh} alt="Sứ Mệnh" className="h-72 object-cover" />
                    </div>
                </div>
            </article>

            <article className="bg-orange-100 p-10 text-center mb-8">
                <h4 className="text-3xl text-custom1 font-bold mb-6">
                    HỆ THỐNG CỬA HÀNG BOOK SHOP
                </h4>
                <div className="max-w-screen-xl mx-auto">
                    <p className="text-gray-700 leading-7 text-justify">
                        Hệ thống cửa hàng là cầu nối quan trọng giữa doanh nghiệp và khách hàng, mang đến trải nghiệm mua sắm tiện lợi và đáng tin cậy.
                        Mỗi cửa hàng được thiết kế đồng nhất, dễ nhận diện thương hiệu, đồng thời cung cấp sản phẩm đa dạng và dịch vụ chuyên nghiệp.
                        Đội ngũ nhân viên luôn sẵn sàng tư vấn, hỗ trợ tận tình, tạo cảm giác thoải mái cho khách hàng.
                    </p>
                </div>

                <div className="grid grid-cols-3 gap-6 mt-8 max-w-screen-xl mx-auto">
                    {[nhaSach1, nhaSach2, nhaSach3].map((image, index) => (
                        <img
                            key={index}
                            src={image}
                            alt={`Hình ${index + 1}`}
                            className="w-full h-full shadow-md object-cover rounded-md"
                        />
                    ))}
                </div>
            </article>

            <article className="p-6">
                <div className="flex items-center justify-between max-w-screen-xl mx-auto">
                    <div className="border border-gray-300 overflow-hidden shadow rounded-md ">
                        <img
                            src={camKet}
                            alt="Cam Kết"
                            className="h-64 object-cover"
                        />
                    </div>
                    <div className="w-1/2">
                        <h2 className="text-3xl font-bold text-custom1 mb-4 text-center">
                            CAM KẾT BÁN HÀNG MỞ RỘNG
                        </h2>
                        <p className="text-gray-700 leading-7 text-justify">
                            Cam kết bán hàng về sách của chúng tôi là mang đến cho khách hàng nguồn sách phong phú,
                            đa dạng thể loại, đáp ứng mọi nhu cầu từ giải trí, học tập đến nghiên cứu chuyên sâu.
                            Mỗi cuốn sách đều được chọn lọc kỹ lưỡng, đảm bảo chất lượng nội dung và hình thức.
                            <br /><br />
                            Chúng tôi cam kết cung cấp dịch vụ tư vấn tận tâm, hỗ trợ khách hàng tìm được sách phù
                            hợp và mang lại trải nghiệm mua sắm thuận tiện, tin cậy.
                        </p>
                    </div>

                </div>
            </article>
        </>
    )
}
export default About;