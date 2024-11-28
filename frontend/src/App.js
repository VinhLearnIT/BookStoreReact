import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LoginPage from './Login/LoginPage';
import AdminPage from './Admin/AdminPage';
function App() {
    return (
        <Router>
            <Routes>
                <Route path="/auth/*" element={<LoginPage />} />
                <Route path="/admin/*" element={<AdminPage />} />
            </Routes>
        </Router>
    );
}

export default App;
