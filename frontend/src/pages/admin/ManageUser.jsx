import React from 'react';
import { Breadcrumb } from 'antd';

const ManageUser = () => {
    return (
        <>
            <Breadcrumb
                items={[
                    { title: 'Admin' },
                    { title: "Quản lý người dùng" }
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

export default ManageUser;