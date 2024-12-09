import React, { createContext, useState } from 'react';
export const CartContext = createContext();

export const CartProvider = ({ children }) => {
    const [cartCount, setCartCount] = useState(0);

    const setCartQuantity = (quantity) => {
        setCartCount(quantity);
    };

    const addToCart = (quantity) => {
        setCartCount((prev) => prev + quantity);
    };

    const removeFromCart = (quantity) => {
        setCartCount((prev) => Math.max(prev - quantity, 0));
    };

    return (
        <CartContext.Provider value={{ cartCount, setCartQuantity, addToCart, removeFromCart }}>
            {children}
        </CartContext.Provider>
    );
};
