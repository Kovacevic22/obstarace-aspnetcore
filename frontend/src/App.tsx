import { BrowserRouter as Router, Routes, Route } from 'react-router';
import HomePage from "./pages/HomePage.tsx";
import LoginPage from "./pages/auth/LoginPage.tsx";
import RegisterPage from "./pages/auth/RegisterPage.tsx";
import RacesPage from "./pages/RacesPage.tsx";
function App() {

  return (
    <Router>
      <Routes>
        <Route path="/" element={<HomePage/>} />
        <Route path="/login" element={<LoginPage/>} />
        <Route path="/register" element={<RegisterPage/>} />
        <Route path="/races" element={<RacesPage/>}/>
      </Routes>
    </Router>
  )
}

export default App
