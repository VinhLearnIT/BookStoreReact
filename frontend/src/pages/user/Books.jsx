import React, { useState, useEffect, useCallback } from "react";
import { App, Slider, Checkbox } from "antd";
import * as bookService from '../../services/BookService';
import * as categoryService from '../../services/CategoryService';
import BookList from '../../components/BooksList';
import slider2 from '../../assets/images/slider2.jpg';

const Books = () => {
    const { message } = App.useApp();
    const [books, setBooks] = useState([]);
    const [filteredBooks, setFilteredBooks] = useState([]);
    const [categories, setCategories] = useState([]);
    const [filters, setFilters] = useState({
        minPrice: 0,
        maxPrice: 1000000,
        selectedCategories: []
    });

    const getBooks = useCallback(async () => {
        try {
            const response = await bookService.GetBooks();
            setBooks(response.data);
            setFilteredBooks(response.data);
        } catch (error) {
            message.error("Không thể tải dữ liệu sách!");
        }
    }, [message]);

    const getCategories = useCallback(async () => {
        try {
            const response = await categoryService.GetCategories();
            setCategories(response.data);
        } catch (error) {
            message.error("Không thể tải dữ liệu thể loại!");
        }
    }, [message]);

    const filterBooks = (updatedFilters) => {
        let filtered = [...books];
        if (updatedFilters.selectedCategories.length > 0) {
            filtered = filtered.filter(book =>
                updatedFilters.selectedCategories.some(condition => book.categories.includes(condition))
            );
        }
        filtered = filtered.filter(book => book.price >= updatedFilters.minPrice && book.price <= updatedFilters.maxPrice);

        setFilteredBooks(filtered);
    };

    useEffect(() => {
        getBooks();
        getCategories()
    }, [getBooks, getCategories]);

    const handleFilterChange = (key, value) => {
        let updatedFilters = {
            ...filters
        };
        if (key === "priceRange") {
            updatedFilters = {
                ...updatedFilters,
                minPrice: value[0],
                maxPrice: value[1],
            };

        } else {
            updatedFilters = {
                ...updatedFilters,
                [key]: value,
            };
        }
        setFilters(updatedFilters);
        filterBooks(updatedFilters);
    };
    const handleCategoryChange = (checkedValues) => {
        handleFilterChange('selectedCategories', checkedValues);
    };

    return (
        <>
            <span
                className="w-full h-[450px] block bg-center"
                style={{ backgroundImage: `url(${slider2})` }}
            />
            <div className="flex max-w-screen-xl mx-auto mt-20 gap-10 min-h-screen">
                <div className="w-1/5 border border-custom1 p-4 h-fit rounded-md shadow-md">
                    <h2 className="text-lg font-bold text-custom1 mb-4 text-center">LỰA CHỌN CỦA BẠN</h2>
                    <div>
                        <h3 className="font-bold mt-6 mb-4 text-custom1">THEO GIÁ:</h3>
                        <Slider
                            range
                            min={0}
                            max={500000}
                            step={10000}
                            defaultValue={[0, 500000]}
                            onChangeComplete={(value) => handleFilterChange("priceRange", value)}
                        />
                        <h3 className="font-bold mt-6 mb-4 text-custom1">THỂ LOẠI:</h3>
                        <Checkbox.Group
                            options={categories.map((category) => ({
                                label: category.categoryName,
                                value: category.categoryName,
                                style: { width: '100%', marginBottom: 15 },
                            }))}
                            value={filters.selectedCategories}
                            onChange={handleCategoryChange}
                        />

                    </div>
                </div>

                <div className="flex-1">
                    <BookList book={filteredBooks} col={4} />
                </div>
            </div>
        </>
    );
};

export default Books;
