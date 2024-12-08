import React from 'react';
import { Modal, Table, Descriptions, Button } from 'antd';

const OrderDetailModal = ({ isModalOpen, order, orderDetail, onClose }) => {
    // Định nghĩa cột cho bảng chi tiết sản phẩm
    const itemColumns = [
        { title: 'Sản phẩm', dataIndex: 'bookName', key: 'bookName', },
        { title: 'Số lượng', dataIndex: 'quantity', key: 'quantity', align: "center" },
        {
            title: 'Đơn giá', dataIndex: 'price', key: 'price', align: "center",
            render: (price) => `${price.toLocaleString()} VNĐ`
        },
        {
            title: 'Thành tiền', dataIndex: 'total', key: 'total', align: "center",
            render: (_, record) => `${(record.quantity * record.price).toLocaleString()} VNĐ`
        },
    ];

    const totalAmount = orderDetail?.reduce((sum, item) => sum + (item.price * item.quantity), 0) || 0;

    return (
        <Modal
            open={isModalOpen}
            onCancel={onClose}
            width={700}
            footer={null}
        >
            <p className='text-center text-xl text-custom1 font-bold mb-4'>CHI TIẾT ĐƠN HÀNG</p>
            <Descriptions bordered column={1} size="small">
                <Descriptions.Item label="Người mua hàng">{order?.fullName}</Descriptions.Item>
                <Descriptions.Item label="Ngày mua hàng">{new Date(order?.orderDate).toLocaleDateString()}</Descriptions.Item>
                <Descriptions.Item label="Địa chỉ">{order?.address}</Descriptions.Item>
                <Descriptions.Item label="Số điện thoại">{order?.phone}</Descriptions.Item>
                <Descriptions.Item label="Thanh toán">
                    {order?.paymentMethod === "COD" ? "Thanh toán khi nhận hàng" : "Đã thanh toán qua MoMo"}
                </Descriptions.Item>
            </Descriptions>

            <Table
                columns={itemColumns}
                dataSource={orderDetail}
                rowKey="orderDetailID"
                pagination={false}
                className="mt-4"
            />

            <div className="text-right text-lg font-medium mt-4">
                <p>Tổng giá trị đơn hàng: {totalAmount.toLocaleString()} VNĐ</p>
            </div>
            <div className="text-right mt-4">
                <Button type='primary' onClick={onClose}>Thoát</Button>
            </div>

        </Modal>
    );
};

export default OrderDetailModal;
