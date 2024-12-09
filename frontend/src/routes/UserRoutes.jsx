import React from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import PrivateRoute from '../components/PrivateRoute';
import { CartProvider } from '../components/CartContext';
import UserLayout from '../layouts/UserLayout'
import Home from '../pages/user/Home';
import About from '../pages/user/About';
import Contact from '../pages/user/Contact';
import Books from '../pages/user/Books';
import BookDetail from '../pages/user/BookDetail';
import Account from '../pages/user/Account';
import Order from '../pages/user/Order';
import ShoppingCart from '../pages/user/ShoppingCart';
import Payment from '../pages/user/Payment';

const AdminRoutes = () => {
    return (
        <CartProvider>
            <Routes>
                <Route element={<UserLayout />}>
                    <Route path="/" element={<Navigate to="/home" />} />
                    <Route path="/home" element={<Home />} />
                    <Route path="/about" element={<About />} />
                    <Route path="/books" element={<Books />} />
                    <Route path="/contact" element={<Contact />} />
                    <Route element={<PrivateRoute allowedRoles={['User', 'Admin', 'Manager']} />}>
                        <Route path="/account" element={<Account />} />
                        <Route path="/order" element={<Order />} />
                    </Route>
                    <Route path="/payment" element={<Payment />} />
                    <Route path="/shopping-cart" element={<ShoppingCart />} />
                    <Route path="/book-detail/:id" element={<BookDetail />} />
                </Route>
            </Routes>
        </CartProvider>
    )
}

export default AdminRoutes;