import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LoginLayout from './Layouts/LoginLayout';
import AdminLayout from './Layouts/AdminLayout';
function App() {
    return (
        <Router>
            <Routes>
                <Route path="/auth/*" element={<LoginLayout />} />
                <Route path="/admin/*" element={<AdminLayout />} />
            </Routes>
        </Router>
    );
}

export default App;
