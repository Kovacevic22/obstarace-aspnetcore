/* eslint-disable @typescript-eslint/no-explicit-any */
import { BrowserRouter as Router, Routes, Route } from 'react-router';
import HomePage from "./pages/HomePage.tsx";
import LoginPage from "./pages/auth/LoginPage.tsx";
import RegisterPage from "./pages/auth/RegisterPage.tsx";
import RacesPage from "./pages/RacesPage.tsx";
import PublicNavbar from "./components/navbar/PublicNavbar.tsx";
import Footer from "./components/common/Footer.tsx";
import {useEffect, useState} from "react";
import {authService} from "./services/authService.ts";
import AdminNavbar from "./components/navbar/AdminNavbar.tsx";
import OrganizerNavbar from "./components/navbar/OrganizerNavbar.tsx";
import UserNavbar from "./components/navbar/UserNavbar.tsx";
import type {UserDto} from "./Models/users.type.ts";
import RaceDetailsPage from "./pages/RaceDetailsPage.tsx";
import AdminDashboard from "./pages/admin/AdminDashboard.tsx";
function App() {
    const [user, setUser] = useState<UserDto|null>(null);
    const [loading, setLoading] = useState(true);
    useEffect(() => {
       const checkAuth= async () => {
           try{
               const response = await authService.me();
               setUser(response);
           }catch (err){
               console.log(err);
               setUser(null);
           }finally {
               setLoading(false);
           }
       };
       void checkAuth();
    },[]);
    const renderNavbar = () => {
        if(user==null)return <PublicNavbar/>;
        const userRole = (user as any).role;
        switch (userRole) {
            case 1:
                return <AdminNavbar />;
            case 2:
                return <OrganizerNavbar />;
            default:
                return <UserNavbar />;
        }
    }
    if (loading)
        return (
            <div className="flex flex-col items-center justify-center min-h-screen bg-dark">
                <div className="relative">
                    <div className="absolute inset-0 rounded-full bg-accent opacity-20 animate-ping"></div>
                    <div className="w-16 h-16 border-4 border-accent/10 border-t-accent rounded-full animate-spin"></div>
                </div>

                <div className="mt-8 flex flex-col items-center gap-2">
                    <div className="text-white font-black uppercase tracking-[0.5em] text-[10px] animate-pulse">
                        Loading <span className="text-accent">Arena</span>
                    </div>
                    <div className="w-32 h-px bg-white/5 relative overflow-hidden">
                        <div className="absolute inset-0 bg-accent w-1/2 animate-[loading-bar_1.5s_infinite_ease-in-out]"></div>
                    </div>
                </div>
            </div>
        );
  return (
    <Router>
        {renderNavbar()}
      <Routes>
          <Route
              path="/"
              element={(user as any)?.role === 1 ? <AdminDashboard /> : <HomePage />}
          />
        <Route path="/login" element={<LoginPage/>} />
        <Route path="/register" element={<RegisterPage/>} />
        <Route path="/races" element={<RacesPage/>}/>
          <Route path="/races/:slug" element={<RaceDetailsPage />} />
          {(user as any)?.role === 1 && (
              <Route path="/admin/dashboard" element={<AdminDashboard />} />
          )}
      </Routes>
        <Footer/>
    </Router>
  )
}

export default App
