import React, { useState, useCallback, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { App, Breadcrumb, Table, Input, Popconfirm, Tooltip, Modal, Form, Button } from 'antd';
import { SearchOutlined, PlusOutlined, EditOutlined, DeleteOutlined, QuestionCircleOutlined } from '@ant-design/icons';
import * as categoryService from '../../services/CategoryService';
import refreshToken from '../../utils/RefreshToken';

const ManageCategory = () => {
    const navigate = useNavigate();
    const { message } = App.useApp();

    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchText, setSearchText] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [form] = Form.useForm();

    const modalType = useRef("add");
    const selectedCategoryID = useRef(null);

    const getCategories = useCallback(async () => {
        try {
            const response = await categoryService.GetCategories();
            setCategories(response.data);
        } catch (error) {
            message.error("Không thể tải dữ liệu thể loại!");
        } finally {
            setLoading(false);
        }
    }, [message]);
    useEffect(() => {
        getCategories();
    }, [getCategories]);

    const showAddModal = () => {
        modalType.current = "add";
        form.resetFields();
        setIsModalOpen(true);
    };

    const showEditModal = (category) => {
        modalType.current = "edit";
        form.setFieldsValue({
            categoryName: category.categoryName,
        });
        setIsModalOpen(true);
    };

    const handleCancelModal = () => {
        setIsModalOpen(false);
        form.resetFields();
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
    const handleAddCategory = async (values) => {
        try {
            let token = localStorage.getItem('accessToken');
            let response = await categoryService.CreateCategory(token, values);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await categoryService.CreateCategory(token, values);
            }

            if (response.status === 200) {
                message.success("Thêm thể loại thành công!");
                await getCategories();
            } else {
                message.error("Thêm thể loại thất bại!");
            }
        } catch (error) {
            message.error("Lỗi khi thêm thể loạ!");
            console.log(error);
        }
    };


    const handleEditCategory = async (values) => {
        try {
            let token = localStorage.getItem('accessToken');
            let response = await categoryService.UpdateCategory(token, selectedCategoryID.current, values);
            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await categoryService.UpdateCategory(token, selectedCategoryID.current, values);
            }

            if (response.status === 200) {
                message.success("Cập nhật thể loại thành công!");
                await getCategories();
            } else {
                message.error("Cập nhật thể loại thất bại!");
            }
        } catch (error) {
            message.error("Lỗi khi cập nhật thể loại!");
            console.log(error);
        }
    };

    const handleSubmit = async (values) => {
        const newCategoryName = values.categoryName.trim().toLowerCase();

        const isDuplicate = categories.some(category =>
            category.categoryName.trim().toLowerCase() === newCategoryName
        );

        if (isDuplicate) {
            message.error("Tên thể loại đã tồn tại! Vui lòng nhập tên khác.");
            return;
        }
        try {
            if (modalType.current === "add") {
                handleAddCategory(values);
            } else {
                handleEditCategory(values);
            }
            setIsModalOpen(false);

        } catch (error) {
            message.error("Có lỗi xảy ra!");
            console.log(error);
        }
    };
    const handleDelete = async () => {
        try {
            let token = localStorage.getItem('accessToken');
            let response = await categoryService.DeleteCategory(token, selectedCategoryID.current);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await categoryService.DeleteCategory(token, selectedCategoryID.current);
            }

            if (response.status === 200) {
                message.success(response.data.message);
                await getCategories();
            } else {
                message.error("Xóa thể loại thất bại!");
            }
        } catch (error) {
            message.error("Lỗi khi xóa thể loại!");
            console.log(error);
        }
    };

    const filteredCategories = categories.filter(categories => {
        var stringSearch = searchText.toLowerCase();
        return String(categories.categoryID) === stringSearch || categories.categoryName.toLowerCase().includes(stringSearch)
    });
    const columns = [
        { title: 'ID', dataIndex: 'categoryID', key: 'categoryID', align: 'center', width: "25%" },
        { title: 'Tên thể loại', dataIndex: 'categoryName', key: 'categoryName', align: 'center', width: "50%" },
        {
            title: 'Thao tác', key: 'actions', align: 'center', width: "25%",
            render: (_, record) => (
                <div className='flex gap-2 justify-center'>
                    <Tooltip title="Cập nhật thể loại" placement='bottom' color={"gold"}>
                        <Button
                            className='px-3 py-5 border-yellow-500 hover:!border-yellow-500'
                            onClick={() => {
                                selectedCategoryID.current = record.categoryID;
                                showEditModal(record);
                            }}>
                            <EditOutlined className='text-yellow-500' />
                        </Button>
                    </Tooltip>

                    <Popconfirm
                        icon={<QuestionCircleOutlined style={{ color: 'red' }} />}
                        title="Xác nhận xóa?"
                        okText="Xác nhận"
                        cancelText="Hủy"
                        onConfirm={() => {
                            selectedCategoryID.current = record.categoryID;
                            handleDelete();
                        }}>
                        <Tooltip title="Xóa thể loại" placement='bottom' color={"red"}>
                            <Button danger className='px-3 py-5'>
                                <DeleteOutlined />
                            </Button>
                        </Tooltip>
                    </Popconfirm>
                </div>
            )
        }
    ];
    return (
        <>
            <Breadcrumb
                items={[
                    { title: 'Admin' },
                    { title: "Quản lý thể loại" }
                ]}
            />
            <div className='bg-white mt-4 p-4 rounded-md shadow-md' style={{ minHeight: 'calc(100vh - 8rem)' }}>
                <div className="flex justify-between mb-4">
                    <Input
                        placeholder="Tìm thể loại..."
                        prefix={<SearchOutlined className='mr-2 ' />}
                        onChange={(e) => setSearchText(e.target.value)}
                        style={{ width: 400 }}
                    />
                    <Button
                        type="primary"
                        onClick={showAddModal}
                    >
                        <PlusOutlined className='mr-2' />
                        Thêm thể loại
                    </Button>
                </div>
                <Table
                    columns={columns}
                    dataSource={filteredCategories}
                    rowKey="categoryID"
                    loading={loading}
                    pagination={{ pageSize: 6 }}
                />
            </div>
            <Modal
                open={isModalOpen}
                onCancel={handleCancelModal}
                footer={null}
                width={400}
                style={{ top: 200 }}
            >
                <p className='text-center text-xl text-custom1 font-bold mb-6'>
                    {modalType.current === "add" ? "THÊM THỂ LOẠI MỚI" : "CẬP NHẬT THỂ LOẠI"}
                </p>
                <Form form={form} onFinish={handleSubmit} layout="vertical"
                    requiredMark={false} className='custom'>
                    <Form.Item
                        name="categoryName"
                        label="Tên thể loại"
                        rules={[{ required: true, message: 'Vui lòng nhập thể loại!' }]}
                    >
                        <Input className='mb-1' placeholder='Nhập thể loại' />
                    </Form.Item>
                    <Form.Item>
                        <Button
                            type='primary'
                            htmlType="submit"
                            className="w-full h-10 mt-2"
                        >
                            {modalType.current === "add" ? "Thêm mới" : "Cập nhật"}
                        </Button>
                    </Form.Item>
                </Form>
            </Modal>
        </>
    )
}

export default ManageCategory;