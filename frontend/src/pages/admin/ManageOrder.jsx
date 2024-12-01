import React from 'react';
import { Breadcrumb } from 'antd';

const ManageOrder = () => {
    return (
        <>
            <Breadcrumb
                items={[
                    { title: 'Admin' },
                    { title: "Quản lý đơn hàng" }
                ]}
            />
            <div
                className='bg-white mt-4 p-4 rounded-md shadow-md'
                style={{ minHeight: 'calc(100vh - 8rem)' }}
            >
                chào
            </div>
        </>
    )
}

export default ManageOrder;