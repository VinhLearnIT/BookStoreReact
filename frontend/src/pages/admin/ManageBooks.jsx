import React, { useEffect, useState, useRef, useCallback } from 'react';
import dayjs from 'dayjs';
import { useNavigate } from 'react-router-dom';
import refreshToken from '../../utils/RefreshToken';
import { App, Breadcrumb, Table, Input, InputNumber, Popconfirm, Tooltip, Modal, Form, Select, Button, DatePicker, Upload, Image }
    from 'antd';
import { SearchOutlined, PlusOutlined, EditOutlined, DeleteOutlined, QuestionCircleOutlined, UploadOutlined }
    from '@ant-design/icons';
import * as bookService from '../../services/BookService';
import * as categoryService from '../../services/CategoryService';

const ManageBooks = () => {
    const { message } = App.useApp();
    const navigate = useNavigate();

    const [books, setBooks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchText, setSearchText] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [form] = Form.useForm();
    const modalType = useRef("add");
    const selectedBookID = useRef(null);
    const selectedBookImage = useRef("");

    const [fileList, setFileList] = useState([]);
    const [previewOpen, setPreviewOpen] = useState(false);
    const [previewImage, setPreviewImage] = useState('');
    const [categoryOptions, setCategoryOptions] = useState([]);

    const getBooks = useCallback(async () => {
        try {
            const response = await bookService.GetBooks();
            setBooks(response.data);
        } catch (error) {
            message.error("Không thể tải dữ liệu sách!");
        } finally {
            setLoading(false);
        }
    }, [message]);

    const getCategories = useCallback(async () => {
        try {
            const response = await categoryService.GetCategories();
            if (response.status === 200) {
                var data = response.data;
                const category = data.reduce((preValue, currentValue) => {
                    preValue.push({
                        label: currentValue.categoryName,
                        value: currentValue.categoryName
                    });
                    return preValue;
                }, []);
                setCategoryOptions(category);
            }
        } catch (error) {
            message.error("Không thể tải dữ liệu thể loại!");
            console.log(error);
        }

    }, [message]);


    useEffect(() => {
        getBooks();
        getCategories();
    }, [getBooks, getCategories]);

    const filteredBooks = books.filter(book => {
        var stringSearch = searchText.toLowerCase();
        return String(book.bookID) === stringSearch || book.bookName.toLowerCase().includes(stringSearch) ||
            book.author.toLowerCase().includes(stringSearch) || book.publisher.toLowerCase().includes(stringSearch) ||
            String(book.price) === stringSearch || String(book.stockQuantity) === stringSearch ||
            book.categories.toLowerCase().includes(stringSearch)
    });

    const getBase64 = (file) =>
        new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result);
            reader.onerror = (error) => reject(error);
        });

    const handleUploadChange = ({ fileList: newFileList }) => {
        setFileList(newFileList);
    };

    const handlePreview = async (file) => {
        if (!file.url && !file.preview) {
            file.preview = await getBase64(file.originFileObj);
        }
        setPreviewImage(file.url || file.preview);
        setPreviewOpen(true);
    };

    const showAddModal = () => {
        modalType.current = "add";
        form.resetFields();
        setIsModalOpen(true);
        setFileList([]);
    };

    const showEditModal = (book) => {
        modalType.current = "edit";
        const categoriesArray = book.categories.split(',').map(item => item.trim());
        const validCategoriesArray = categoriesArray.filter(category =>
            categoryOptions.some(option => option.value === category)
        );
        form.setFieldsValue({
            bookName: book.bookName,
            author: book.author,
            publisher: book.publisher,
            publishedDate: dayjs(book.publishedDate),
            price: book.price,
            stockQuantity: book.stockQuantity,
            description: book.description,
            categories: validCategoriesArray,
        });
        if (book.imagePath) {
            setFileList([
                {
                    uid: '-1',
                    name: book.imagePath,
                    status: 'done',
                    url: `https://localhost:7138/api/images/${book.imagePath}`,
                },
            ]);
        }
        setIsModalOpen(true);
    };

    const handleCancelModal = () => {
        setIsModalOpen(false);
        form.resetFields();
        setFileList([]);
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

    const handleUploadImage = async () => {
        if (!fileList || fileList.length === 0) {
            message.error("Vui lòng chọn ảnh!");
            return null;
        }
        try {
            let token = localStorage.getItem('accessToken');
            const formData = new FormData();
            formData.append('imageFile', fileList[0].originFileObj);
            let response = await bookService.UploadImage(token, formData);
            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await bookService.UploadImage(token, formData);
            }

            if (response.status === 200) {
                return response.data.imagePath;
            } else {
                message.error("Không thể upload hình ảnh!");
            }
        } catch (error) {
            message.error("Lỗi khi upload hình ảnh!");
            console.log(error);
            return null;
        }
    }

    const handleAddBook = async (values) => {
        try {
            const imagePath = await handleUploadImage();
            if (!imagePath) {
                return;
            }
            values.imagePath = imagePath;
            let token = localStorage.getItem('accessToken');
            let response = await bookService.CreateBook(token, values);
            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await bookService.CreateBook(token, values);
            }
            if (response.status === 200) {
                message.success("Thêm mới sách thành công!");
                await getBooks();
            } else {
                message.error("Thêm sách mới thất bại!");
            }
        } catch (error) {
            message.error("Lỗi khi thêm sách!");
            console.log(error);
        }
    }

    const handleEditBook = async (values) => {
        try {
            values.imagePath = selectedBookImage.current;
            if (values.imagePath !== fileList[0].name) {
                const imagePath = await handleUploadImage();
                values.imagePath = imagePath;
            }
            let token = localStorage.getItem('accessToken');
            let response = await bookService.UpdateBook(token, selectedBookID.current, values);

            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await bookService.UpdateBook(token, selectedBookID.current, values);
            }
            if (response.status === 200) {
                message.success("Cập nhật sách thành công!");
                await getBooks();
            } else {
                message.error("Cập nhật sách thất bại!");
            }
        } catch (error) {
            message.error("Lỗi khi cập nhật sách!");
            console.log(error);
        }
    }

    const handleSubmit = async (values) => {
        try {
            values.publishedDate = values.publishedDate.format('YYYY-MM-DD');
            values.categories = values.categories.join(', ');

            if (modalType.current === "add") {
                handleAddBook(values);
            } else {
                handleEditBook(values);
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
            let response = await bookService.DeleteBook(token, selectedBookID.current);
            if (response.status === 401) {
                token = await refreshAccessToken();
                if (!token) return;
                response = await bookService.DeleteBook(token, selectedBookID.current);
            }

            if (response.status === 200) {
                message.success(response.data.message);
                await getBooks();
            } else {
                message.warning(response.message || "Sách này có dữ liệu đã xử dụng không thể xóa!");
            }
        } catch (error) {
            message.error("Lỗi khi xóa sách!");
            console.log(error);
        }
    };

    const columns = [
        { title: 'ID', dataIndex: 'bookID', key: 'bookID', align: 'center', width: 50, },
        {
            title: 'Ảnh', dataIndex: 'imagePath', key: 'imagePath', align: 'center', width: 80,
            render: (path) =>
                <img
                    src={`https://localhost:7138/api/images/${path}`}
                    alt="Book"
                    className='w-full h-12 object-cover rounded-md border border-custom1 p-1' />
        },
        {
            title: 'Tên sách', dataIndex: 'bookName', key: 'bookName', align: 'center', width: 120, ellipsis: true,
            render: (bookName) => (
                <Tooltip placement="topLeft" title={bookName}>
                    {bookName}
                </Tooltip>
            ),
        },
        {
            title: 'Tác giả', dataIndex: 'author', key: 'author', align: 'center', width: 120, ellipsis: true,
            render: (author) => (
                <Tooltip placement="topLeft" title={author}>
                    {author}
                </Tooltip>
            ),
        },
        {
            title: 'Nhà xuất bản', dataIndex: 'publisher', key: 'publisher', align: 'center', width: 120, ellipsis: true,
            render: (publisher) => (
                <Tooltip placement="topLeft" title={publisher}>
                    {publisher}
                </Tooltip>
            ),
        },
        {
            title: 'Ngày xuất bản', dataIndex: 'publishedDate', key: 'publishedDate', align: 'center', width: 130,
            ellipsis: true,
            render: (date) => (
                <Tooltip placement="topLeft" title={new Date(date).toLocaleDateString()}>
                    {new Date(date).toLocaleDateString()}
                </Tooltip>
            ),
        },
        {
            title: 'Giá', dataIndex: 'price', key: 'price', align: 'center', width: 120, ellipsis: true,
            sorter: (a, b) => a.price - b.price, sortDirections: ['descend'],
            render: (price) => (
                <Tooltip placement="topLeft" title={`${price} VNĐ`}>
                    {price.toLocaleString()} VNĐ
                </Tooltip>
            ),
        },
        {
            title: 'Số lượng', dataIndex: 'stockQuantity', key: 'stockQuantity', align: 'center',
            sorter: (a, b) => a.stockQuantity - b.stockQuantity, sortDirections: ['descend'],
        },
        {
            title: 'Mô tả', dataIndex: 'description', key: 'description', align: 'center', ellipsis: true,
            render: (description) => (
                <Tooltip placement="topLeft" title={description}>
                    {description}
                </Tooltip>
            ),
        },
        {
            title: 'Thể loại', dataIndex: 'categories', key: 'categories', align: 'center', width: 120, ellipsis: true,
            render: (categories) => (
                <Tooltip placement="topLeft" title={categories}>
                    {categories}
                </Tooltip>
            ),
        },
        {
            title: 'Thao tác', key: 'actions', align: 'center', width: 110,
            render: (_, record) => (
                <div className='flex gap-2 justify-center'>
                    <Tooltip title="Cập nhật sách" placement='bottom' color={"gold"}>
                        <Button
                            className='px-3 py-5 border-yellow-500 hover:!border-yellow-500'
                            onClick={() => {
                                selectedBookID.current = record.bookID;
                                selectedBookImage.current = record.imagePath;
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
                            selectedBookID.current = record.bookID;
                            handleDelete();
                        }}>
                        <Tooltip title="Xóa sách" placement='bottom' color={"red"}>
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
                    { title: 'Quản lý sách' }
                ]}
            />
            <div className='bg-white mt-4 p-4 rounded-md shadow-md' style={{ minHeight: 'calc(100vh - 8rem)' }}>
                <div className="flex justify-between mb-4">
                    <Input
                        placeholder="Tìm kiếm sách..."
                        prefix={<SearchOutlined className='mr-2 ' />}
                        onChange={(e) => setSearchText(e.target.value)}
                        style={{ width: 400 }}
                    />
                    <Button
                        type="primary"
                        onClick={showAddModal}
                    >
                        <PlusOutlined className='mr-2' />
                        Thêm sách mới
                    </Button>
                </div>
                <Table
                    columns={columns}
                    dataSource={filteredBooks}
                    rowKey="bookID"
                    loading={loading}
                    pagination={{ pageSize: 6 }}
                />
            </div>
            <Modal
                open={isModalOpen}
                onCancel={handleCancelModal}
                footer={null}
                width={600}
                style={{ top: 20 }}
            >
                <p className='text-center text-xl text-custom1 font-bold mb-4'>
                    {modalType.current === "add" ? "THÊM SÁCH MỚI" : "CẬP NHẬT SÁCH"}
                </p>
                <Form form={form} onFinish={handleSubmit} layout="horizontal" requiredMark={false} labelCol={{ span: 6 }}>
                    <Form.Item
                        name="bookName"
                        label="Tên sách"
                        rules={[{ required: true, message: 'Vui lòng nhập tên sách!' }]}
                    >
                        <Input className='mb-1 mt-2' placeholder='Nhập tên sách' />
                    </Form.Item>

                    <Form.Item
                        name="author"
                        label="Tác giả"
                        rules={[{ required: true, message: 'Vui lòng nhập tác giả!' }]}
                    >
                        <Input className='mb-1 mt-2' placeholder='Nhập tên tác giả' />
                    </Form.Item>

                    <Form.Item
                        name="publisher"
                        label="Nhà xuất bản"
                        rules={[{ required: true, message: 'Vui lòng nhập nhà xuất bản!' }]}
                    >
                        <Input className='mb-1 mt-2' placeholder='Nhập tên nhà xuất bản' />
                    </Form.Item>

                    <Form.Item
                        name="publishedDate"
                        label="Ngày xuất bản"
                        rules={[{ required: true, message: 'Vui lòng nhập ngày xuất bản!' }]}
                    >
                        <DatePicker
                            format="DD-MM-YYYY"
                            className='w-full mb-1 mt-2 '
                            placeholder='Chọn ngày xuất bản'
                        />
                    </Form.Item>

                    <Form.Item
                        name="price"
                        label="Giá"
                        rules={[{ required: true, message: 'Vui lòng nhập giá!' }]}
                    >
                        <InputNumber min={1} className='mb-1 mt-2  w-full' placeholder='Nhập giá sách' />
                    </Form.Item>

                    <Form.Item
                        name="stockQuantity"
                        label="Số lượng"
                        rules={[{ required: true, message: 'Vui lòng nhập số lượng!' }]}
                    >
                        <InputNumber min={1} className='mb-1 mt-2 w-full' placeholder='Nhập số lượng sách' />
                    </Form.Item>

                    <Form.Item
                        name="categories"
                        label="Thể loại"
                        rules={[{ required: true, message: 'Vui lòng chọn thể loại!' }]}
                    >
                        <Select
                            mode="multiple"
                            placeholder="Chọn thể loại"
                            style={{ width: '100%' }}
                            className='mb-1 mt-2'
                            options={categoryOptions}
                        />
                    </Form.Item>
                    <Form.Item
                        name="description"
                        label="Mô tả"
                        rules={[{ required: true, message: 'Vui lòng nhập mô tả sách!' }]}
                    >
                        <Input.TextArea className='mb-1 mt-2' placeholder='Nhập mô tả sách' rows={3} />
                    </Form.Item>
                    <Form.Item
                        name="imagePath"
                        label="Hình ảnh"
                        rules={[
                            {
                                validator: (_, value) => {
                                    if (fileList.length === 0) {
                                        return Promise.reject(new Error('Vui lòng chọn hình ảnh!'));
                                    }
                                    return Promise.resolve();
                                },
                            },
                        ]}
                    >
                        <Upload
                            beforeUpload={() => false}
                            fileList={fileList}
                            onPreview={handlePreview}
                            onChange={handleUploadChange}
                            accept="image/*"
                            listType="picture"
                        >
                            {fileList.length < 1 && (
                                <Button className="mt-2 mb-1" icon={<UploadOutlined />}>
                                    Click to Upload
                                </Button>
                            )}
                        </Upload>
                    </Form.Item>
                    {previewImage && (
                        <Image
                            wrapperStyle={{
                                display: 'none',
                            }}
                            preview={{
                                visible: previewOpen,
                                onVisibleChange: (visible) => setPreviewOpen(visible),
                                afterOpenChange: (visible) => !visible && setPreviewImage(''),
                            }}
                            src={previewImage}
                        />
                    )}
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
    );
};

export default ManageBooks;
