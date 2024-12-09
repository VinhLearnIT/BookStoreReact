import React, { useEffect, useState, useCallback, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import refreshToken from '../../utils/RefreshToken';
import { Breadcrumb, Select, App, Image } from 'antd';
import { Bar, Line } from 'react-chartjs-2';
import {
    Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend, PointElement, LineElement, Filler
} from 'chart.js';
import * as statisticsService from '../../services/StatisticsService';

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend, PointElement, LineElement, Filler);

const { Option } = Select;

const DashBoard = () => {
    const navigate = useNavigate();
    const { message } = App.useApp();
    const [statistics, setStatistics] = useState(null);
    const [revenueData, setRevenueData] = useState([]);
    const [salesData, setSalesData] = useState([]);
    const year = useRef(new Date().getFullYear());
    const refreshAccessToken = useCallback(async () => {
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
    }, [message, navigate]);

    const getMonthlyRevenueOfYear = useCallback(async (year) => {
        try {
            let token = localStorage.getItem('accessToken');
            let response = await statisticsService.GetMonthlyRevenueOfYear(token, year);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await statisticsService.GetMonthlyRevenueOfYear(token, year);
            }
            if (response.status === 200) {
                setRevenueData(response.data);
            } else {
                message.error("Không thể thống kê doanh thu hằng tháng!");
            }
        } catch (error) {
            message.error("Lỗi khi thống kê doanh thu hằng tháng!");
            console.log(error);
        }
    }, [message, refreshAccessToken]);

    const getMonthlySalesOfYear = useCallback(async (year) => {
        try {
            let token = localStorage.getItem('accessToken');
            let response = await statisticsService.GetMonthlySalesOfYear(token, year);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await statisticsService.GetMonthlySalesOfYear(token, year);
            }
            if (response.status === 200) {
                setSalesData(response.data);
            } else {
                message.error("Không thể thống kê số lượng bán hằng tháng!");
            }
        } catch (error) {
            message.error("Lỗi khi thống kê số lượng bán hằng tháng!");
            console.log(error);
        }
    }, [message, refreshAccessToken]);

    const getStatistics = useCallback(async () => {
        try {
            let token = localStorage.getItem('accessToken');
            let response = await statisticsService.GetMonthlyStatistics(token);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await statisticsService.GetMonthlyStatistics(token);
            }

            if (response.status === 200) {
                setStatistics(response.data);
            } else {
                message.error("Không thể thống kê tháng hiện tại!");
            }
        } catch (error) {
            message.error("Lỗi khi thống kê tháng hiện tại!");
            console.log(error);
        }
    }, [message, refreshAccessToken]);

    useEffect(() => {
        getStatistics();
        getMonthlyRevenueOfYear(year.current);
        getMonthlySalesOfYear(year.current);
    }, [getStatistics, getMonthlyRevenueOfYear, getMonthlySalesOfYear]);

    const revenueChartData = {
        labels: revenueData.map((data) => `${data.month}`),
        datasets: [
            {
                label: 'Doanh thu',
                data: revenueData.map((data) => data.totalRevenue),
                backgroundColor: 'rgba(75, 192, 192, 0.6)',
            },
        ],
    };

    const salesChartData = {
        labels: salesData.map((data) => `${data.month}`),
        datasets: [
            {
                label: 'Số lượng bán',
                data: salesData.map((data) => data.totalSales),
                borderColor: 'rgba(153, 102, 255, 1)',
                backgroundColor: 'rgba(153, 102, 255, 0.2)',
                fill: true,
            },
        ],
    };

    return (
        <>
            <Breadcrumb
                items={[
                    { title: 'Admin' },
                    { title: 'Trang chủ' }
                ]}
            />
            <div className='bg-white mt-4 p-4 rounded-md shadow-md' style={{ minHeight: 'calc(100vh - 8rem)' }}>
                <h2 className="text-2xl font-bold text-custom1 text-center mb-8 mt-1">THỐNG KÊ THEO HIỆN TẠI</h2>
                <div className='grid grid-cols-4 gap-4'>
                    <div className="bg-cyan-600 p-6 rounded-lg text-white text-center shadow-md font-bold">
                        <h2 className="text-lg mb-2">Tổng đơn hàng của tháng</h2>
                        <span className="text-2xl ">{statistics?.totalOrders}</span>
                    </div>
                    <div className="bg-blue-600 p-6 rounded-lg text-white text-center shadow-md font-bold">
                        <h2 className="text-lg mb-2">Đơn hàng chờ xác nhận</h2>
                        <span className="text-2xl">{statistics?.pendingOrders}</span>
                    </div>
                    <div className="bg-purple-600 p-6 rounded-lg text-center text-white shadow-md font-bold">
                        <h2 className="text-lg  mb-2">Doanh thu tháng</h2>
                        <span className="text-2xl d">{statistics?.totalRevenue}</span>
                    </div>
                    <div className="bg-yellow-600 p-6 rounded-lg text-center text-white shadow-md font-bold">
                        <h2 className="text-lg mb-2">Tổng người dùng hiện tại</h2>
                        <span className="text-2xl">{statistics?.totalUsers}</span>
                    </div>
                </div>
                <h2 className="text-2xl font-bold text-custom1 text-center my-8">THỐNG KÊ THEO KẾT QUẢ CẢ NĂM</h2>
                <div className='grid grid-cols-2 gap-8'>
                    <div>
                        <div className='flex justify-between items-center'>
                            <h2 className='text-lg font-bold text-custom1 mb-2'>
                                Doanh thu của tháng trong năm
                            </h2>
                            <Select
                                defaultValue={year.current}
                                onChange={(value) => getMonthlyRevenueOfYear(value)}
                                style={{ width: 120 }}
                            >
                                {[...Array(5)].map((_, index) => {
                                    const pastYear = year.current - index;
                                    return (
                                        <Option key={pastYear} value={pastYear}>
                                            {pastYear}
                                        </Option>
                                    );
                                })}
                            </Select>
                        </div>
                        <Bar data={revenueChartData} />
                    </div>
                    <div>
                        <div className="flex justify-between items-center">
                            <h2 className='text-lg font-bold text-custom1 mb-2'>
                                Số lượng sách bán được của tháng trong năm
                            </h2>
                            <Select
                                defaultValue={year.current}
                                onChange={(value) => getMonthlySalesOfYear(value)}
                                style={{ width: 120 }}
                            >
                                {[...Array(5)].map((_, index) => {
                                    const pastYear = year.current - index;
                                    return (
                                        <Option key={pastYear} value={pastYear}>
                                            {pastYear}
                                        </Option>
                                    );
                                })}
                            </Select>
                        </div>
                        <Line data={salesChartData} />
                    </div>
                </div>
                <h2 className="text-2xl font-bold text-custom1 text-center my-8">TOP 3 SÁCH BÁN CHẠY NHẤT</h2>
                <div className='grid grid-cols-3 my-6 gap-4'>
                    {statistics?.topSellingBooks.map(item =>
                        <div key={item.bookID} className='flex items-center gap-4 border border-custom1 p-2 rounded-md shadow'>
                            <Image
                                width={80}
                                src={`https://localhost:7138/api/images/${item.imagePath}`}
                                className='rounded-md'
                            />
                            <div>
                                <div className='truncate'>
                                    Tên sách: {" "}<span className='font-bold'>{item.bookName}</span>
                                </div>
                                <div className='truncate mt-2'>
                                    Số lượng bán: {" "}
                                    <span className='font-bold'>{item.totalQuantitySold}</span>
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        </>
    );
};

export default DashBoard;
