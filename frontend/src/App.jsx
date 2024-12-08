import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { ConfigProvider, App as AntdApp } from 'antd';

import AuthRoutes from './routes/AuthRoutes';
import AdminRoutes from './routes/AdminRoutes';
import UserRoutes from './routes/UserRoutes';

function App() {
    const cofigTheme =
    {
        token: {
            colorPrimary: '#26648E',
            borderRadius: 6,
            colorBorder: '#26648E',
            fontSize: 16
        },
        components: {
            Form: {
                labelFontSize: 16,
            },
            Button: {
                primaryShadow: "none"
            },
            Table: {
                cellPaddingInline: 8,
                cellPaddingBlock: 10
            },
            Dropdown: {
                paddingBlock: 6
            }
        }
    }

    return (
        <ConfigProvider theme={cofigTheme}>
            <AntdApp>
                <Router>
                    <Routes>
                        <Route path="/*" element={<UserRoutes />} />
                        <Route path="/auth/*" element={<AuthRoutes />} />
                        <Route path="/admin/*" element={<AdminRoutes />} />
                    </Routes>
                </Router>
            </AntdApp>
        </ConfigProvider>
    );
}

export default App;
